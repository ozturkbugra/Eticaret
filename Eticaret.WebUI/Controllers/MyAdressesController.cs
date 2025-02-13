using Eticaret.Core.Entities;
using Eticaret.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Eticaret.WebUI.Controllers
{
    [Authorize]
    public class MyAdressesController : Controller
    {
        private readonly IService<Address> _serviceAddress;
        private readonly IService<AppUser> _serviceAppUser;

        public MyAdressesController(IService<Address> service , IService<AppUser> service1 )
        {
            _serviceAddress = service;
            _serviceAppUser = service1;
        }
        public  async Task<IActionResult> Index()
        {
            var appUser = await _serviceAppUser.GetAsync(x=> x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null) { 
            return NotFound("Kullanıcı Bulunamadı!");

            }

            var model = await _serviceAddress.GetAllAsync(x=> x.AppUserID == appUser.ID);
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Address address)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
                    if(appUser != null)
                    {
                        address.AppUserID = appUser.ID;
                        _serviceAddress.Add(address);
                        await _serviceAddress.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "HATA Oluştu");

                }

            }
            ModelState.AddModelError("", "Kayıt Başarısız");

            return View(address);
        }


        public async Task<IActionResult> Edit(string id)
        {
            var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null)
            {
                return NotFound("Kullanıcı Bulunamadı!");

            }
            var model = await _serviceAddress.GetAsync(x => x.AddressGuid.ToString() == id && x.AppUserID == appUser.ID);
            if (model == null) {
                return NotFound("Adres Bulunamadı!");

            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Address address)
        {
            var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null)
            {
                return NotFound("Kullanıcı Bulunamadı!");

            }
            var model = await _serviceAddress.GetAsync(x => x.AddressGuid.ToString() == id && x.AppUserID == appUser.ID);
            if (model == null)
            {
                return NotFound("Adres Bulunamadı!");

            }

            model.Title = address.Title;
            model.District = address.District;
            model.City = address.City;
            model.OpenAddress = address.OpenAddress;
            model.IsDeliveryAddress = address.IsDeliveryAddress;
            model.IsBillingAddress = address.IsBillingAddress;
            model.IsActive = address.IsActive;

            var otheradress = await _serviceAddress.GetAllAsync(x=> x.AppUserID == appUser.ID && x.ID != model.ID);

            foreach (var item in otheradress) {

                item.IsDeliveryAddress = false;
                item.IsBillingAddress = false;
                _serviceAddress.Update(item);


            }

            try
            {
                _serviceAddress.Update(model);
                await _serviceAddress.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                ModelState.AddModelError("", "Hata Oluştu!");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null)
            {
                return NotFound("Kullanıcı Bulunamadı!");

            }
            var model = await _serviceAddress.GetAsync(x => x.AddressGuid.ToString() == id && x.AppUserID == appUser.ID);
            if (model == null)
            {
                return NotFound("Adres Bulunamadı!");

            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, Address address)
        {
            var appUser = await _serviceAppUser.GetAsync(x => x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appUser == null)
            {
                return NotFound("Kullanıcı Bulunamadı!");

            }
            var model = await _serviceAddress.GetAsync(x => x.AddressGuid.ToString() == id && x.AppUserID == appUser.ID);
            if (model == null)
            {
                return NotFound("Adres Bulunamadı!");

            }

            try
            {
                _serviceAddress.Delete(model);
                await _serviceAddress.SaveChangesAsync();
                return RedirectToAction("Index");   
            }
            catch (Exception)
            {

                ModelState.AddModelError("", "Hata Oluştu!");

            }
            return View(model);
        }
    }
}
