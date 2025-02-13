using System.Security.Claims;
using Eticaret.Core.Entities;
using Eticaret.Data;
using Eticaret.Service.Abstract;
using Eticaret.WebUI.Models;
using Eticaret.WebUI.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Eticaret.WebUI.Controllers
{
    public class AccountController : Controller
    {
        
        //Eski yapı
        //private readonly DatabaseContext _context;

        //public AccountController(DatabaseContext context)
        //{
        //    _context = context;
        //}

        //Yeni yapı
        private readonly IService<AppUser> _service;
        private readonly IService<Order> _serviceOrder;

        public AccountController(IService<AppUser> service, IService<Order> serviceOrder = null)
        {
            _service = service;
            _serviceOrder = serviceOrder;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            AppUser appuser = await _service.GetAsync(x => x.UserGuid.ToString() == 
            HttpContext.User.FindFirst("UserGuid").Value);
            if (appuser is null) {

                return NotFound();
            }
            var model = new UserEditViewModel()
            {
                Email = appuser.Email,
                ID = appuser.ID,
                Name = appuser.Name,
                Surname = appuser.Surname,
                Password = appuser.Password,
                Phone = appuser.Phone,
            };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Index(UserEditViewModel model)
        {
            if (ModelState.IsValid) {
                try
                {
                    AppUser appuser = await _service.GetAsync(x => x.UserGuid.ToString() ==
                    HttpContext.User.FindFirst("UserGuid").Value);
                    if (appuser is not null)
                    {
                        appuser.Surname = model.Surname;
                        appuser.Password = model.Password;
                        appuser.Phone = model.Phone;
                        appuser.Name = model.Name;
                        appuser.Email = model.Email;
                        _service.Update(appuser);
                        _service.SaveChanges();
                    }
                    

                }
                catch (Exception)
                {

                    ModelState.AddModelError("", "Hata Oluştu");
                }
            }
           return View(model);
            
        }

        public IActionResult SingIn()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> MyOrders()
        {
            AppUser appuser = await _service.GetAsync(x => x.UserGuid.ToString() ==
            HttpContext.User.FindFirst("UserGuid").Value);
            if (appuser is null)
            {
                await HttpContext.SignOutAsync();

                return RedirectToAction("SingIn");
            }
            var model = _serviceOrder.GetQueryable().Where(x => x.AppUserID == appuser.ID).Include(x=> x.OrderLines).ThenInclude(x=> x.Product);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SingIn(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var account = await _service.GetAsync(x=> x.Email == loginViewModel.Email && x.Password == loginViewModel.Password && x.IsActive);
                    if (account == null)
                    {
                        ModelState.AddModelError("", "GİRİŞ BAŞARISIZ!");

                    }
                    else
                    {
                        var claims = new List<Claim>()
                        {
                            new(ClaimTypes.Name, account.Name),
                            new(ClaimTypes.Role, account.IsAdmin ? "Admin" : "Customer"),
                            new(ClaimTypes.Email, account.Email),
                            new("UserID", account.ID.ToString()),
                            new("UserGuid", account.UserGuid.ToString())
                        };

                        var userIdentity = new ClaimsIdentity(claims, "Login");

                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(userIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return Redirect(string.IsNullOrEmpty(loginViewModel.ReturnUrl) ? "/" : loginViewModel.ReturnUrl);

                    }

                }
                catch (Exception hata)
                {

                    ModelState.AddModelError("","HATA OLUŞTU!");
                }
            }

            return View(loginViewModel);
        }

        public IActionResult SingUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SingUp(AppUser appUser)
        {
            appUser.IsAdmin = false;
            appUser.IsActive = true;
            if (ModelState.IsValid)
            {
                await _service.AddAsync(appUser);
                await _service.SaveChangesAsync();
            
                return RedirectToAction(nameof(Index));
            }
            return View(appUser);
        }

        public async Task<IActionResult> SignOutAsync()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("SingIn");
        }
        public IActionResult PasswordRenew()
        {
            return View();
        } 
        
        [HttpPost]
        public async Task<IActionResult> PasswordRenew(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                ModelState.AddModelError("", "Email Boş Geçilemez!");
                return View();
            }
            AppUser appuser = await _service.GetAsync(x => x.Email == Email);
            if (appuser is null)
            {

                ModelState.AddModelError("", "Geçersiz Mail!");
                return View();

            }

            string mesaj = $"Sayın {appuser.Name} {appuser.Surname} <br> Şifrenizi Yenilemek için Lütfen " +
                $" <a href='/Account/PasswordChange?user{appuser.UserGuid.ToString()}'> buraya tıklayınız</a>";

            var sonuc = await MailHelper.SendMailAsync(Email, "Şifre Yenileme", mesaj);
            if (sonuc)
            {
                TempData["Message"] = "Mail Başarılı Bir Şekilde Gönderildi";
            }
            return View();
        }


        public async Task<IActionResult> PasswordChange(string user)
        {

            if (user == null) {

                return BadRequest("Geçersiz İstek!");
            }

            AppUser appuser = await _service.GetAsync(x=> x.UserGuid.ToString() == user);
            if (appuser is null)
            {

                return NotFound("Geçersiz Kullanıcı!");

            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(string user, string Password)
        {

            if (user == null) {

                return BadRequest("Geçersiz İstek!");
            }

            AppUser appuser = await _service.GetAsync(x=> x.UserGuid.ToString() == user);
            if (appuser is null)
            {

                ModelState.AddModelError("", "Geçersiz Kullanıcı!");
                return View();

            }
            appuser.Password = Password;
            var sonuc = await _service.SaveChangesAsync();

            if (sonuc > 0)
            {
                TempData["Message"] = "Başarılı";
            }
            else
            {
                ModelState.AddModelError("", "Güncelleme Başarısız!");

            }

            return View();
        }
    }
}
