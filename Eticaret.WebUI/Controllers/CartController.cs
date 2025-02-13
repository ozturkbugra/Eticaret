using Eticaret.Core.Entities;
using Eticaret.Service.Abstract;
using Eticaret.Service.Concrete;
using Eticaret.WebUI.ExtensionMethod;
using Eticaret.WebUI.Models;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Address = Eticaret.Core.Entities.Address;

namespace Eticaret.WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IService<Product> _service;
        private readonly IService<Address> _serviceAddress;
        private readonly IService<AppUser> _serviceAppUser;
        private readonly IService<Order> _serviceOrder;
        private readonly IConfiguration _configuration;


        public CartController(IService<Product> service, IService<Address> serviceAddress, IService<AppUser> serviceAppUser = null, IService<Order> serviceOrder = null, IConfiguration configuration = null)
        {
            _service = service;
            _serviceAddress = serviceAddress;
            _serviceAppUser = serviceAppUser;
            _serviceOrder = serviceOrder;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            var cart = GetCart();
            var model = new CartViewModels()
            {
                CartLines = cart.CartLines,
                TotalPrice = cart.ToTalPrice()
            };
            return View(model);
        }

        public IActionResult Add(int ProductID, int Quantity=1)
        {
            var product = _service.Find(ProductID);
            if (product != null) 
            {
                var cart = GetCart();
                cart.AddProduct(product, Quantity);
                HttpContext.Session.SetJson("Cart", cart);
                return Redirect(Request.Headers["Referer"].ToString());
            }
            return RedirectToAction("Index");
        }


        public IActionResult Update(int ProductID, int Quantity = 1)
        {
            var product = _service.Find(ProductID);
            if (product != null)
            {
                var cart = GetCart();
                cart.UpdateProduct(product, Quantity);
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        
        public IActionResult Remove(int ProductID)
        {
            var product = _service.Find(ProductID);
            if (product != null)
            {
                var cart = GetCart();
                cart.RemoveProduct(product);
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        public  async Task<IActionResult> Checkout()
        {
            var cart = GetCart();
            var appuser = await _serviceAppUser.GetAsync(x=> x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appuser == null) {
                return RedirectToAction("SingIn", "Account");
            }
            var addresses = await _serviceAddress.GetAllAsync(x => x.AppUserID == appuser.ID && x.IsActive);
            var model = new CheckoutModelViews()
            {
                CartProducts = cart.CartLines,
                TotalPrice = cart.ToTalPrice(),
                Addresses = addresses
            };
            return View(model);
        }
        
        [Authorize, HttpPost]
        public  async Task<IActionResult> Checkout(string CardNumber, string CardNameSurname, string CardMonth, string CardYear, 
            string CVV, string DeliveryAddress, string BillingAddress)
        {
            var cart = GetCart();
            var appuser = await _serviceAppUser.GetAsync(x=> x.UserGuid.ToString() == HttpContext.User.FindFirst("UserGuid").Value);
            if (appuser == null) {
                return RedirectToAction("SingIn", "Account");
            }
            var addresses = await _serviceAddress.GetAllAsync(x => x.AppUserID == appuser.ID && x.IsActive);
            var model = new CheckoutModelViews()
            {
                CartProducts = cart.CartLines,
                TotalPrice = cart.ToTalPrice(),
                Addresses = addresses
            };

            if (string.IsNullOrEmpty(CardNumber) || string.IsNullOrEmpty(CardMonth) || string.IsNullOrEmpty(CardYear) || string.IsNullOrEmpty(BillingAddress) || string.IsNullOrEmpty(DeliveryAddress) || string.IsNullOrEmpty(CVV)) { 
            
                return View(model);
            }

            var teslimatAdresi = addresses.FirstOrDefault(x=> x.AddressGuid.ToString() == DeliveryAddress);
            var faturaadresi = addresses.FirstOrDefault(x=> x.AddressGuid.ToString() == BillingAddress);

            //Ödeme Çekme

            var siparis = new Order
            {
                AppUserID = appuser.ID,
                BillingAddress = $"{faturaadresi.OpenAddress} {faturaadresi.District} {faturaadresi.City}",//BillingAddress,
                CustomerID = appuser.UserGuid.ToString(),
                DeliveryAddress = $"{faturaadresi.OpenAddress} {faturaadresi.District} {faturaadresi.City}",//DeliveryAddress,
                OrderDate = DateTime.Now,
                TotalPrice = cart.ToTalPrice(),
                OrderNumber = Guid.NewGuid().ToString(),
                OrderState = 0,
                OrderLines = []
            };

           
            #region OdemeIslemi
            Options options = new Options();
            options.ApiKey = _configuration["IyzicOptions:ApiKey"];
            options.SecretKey = _configuration["IyzicOptions:SecretKey"];
            options.BaseUrl = _configuration["IyzicOptions:BaseUrl"];

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = HttpContext.Session.Id;
            request.Price = siparis.TotalPrice.ToString().Replace(",",".");
            request.PaidPrice = siparis.TotalPrice.ToString().Replace(",", ".");
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B" + HttpContext.Session.Id;
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = CardNameSurname;
            paymentCard.CardNumber = CardNumber; //"5528790000000008"
            paymentCard.ExpireMonth = CardMonth; //"12"
            paymentCard.ExpireYear = CardYear; // "2030"
            paymentCard.Cvc = CVV; //"123"
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = "BY" + appuser.ID;
            buyer.Name = appuser.Name;
            buyer.Surname = appuser.Surname;
            buyer.GsmNumber = appuser.Phone;
            buyer.Email = appuser.Email;
            buyer.IdentityNumber = "111111111111";
            buyer.LastLoginDate = DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss"); // "2015-10-05 12:43:35";
            buyer.RegistrationDate = appuser.CreateDate.ToString("yyyy-mm-dd hh:mm:ss");
            buyer.RegistrationAddress = siparis.DeliveryAddress;
            buyer.Ip = HttpContext.Connection.RemoteIpAddress?.ToString(); //"85.34.78.112";
            buyer.City = teslimatAdresi.City;
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            var shippingAddress = new Iyzipay.Model.Address();
            shippingAddress.ContactName = appuser.Name + " "  + appuser.Surname;
            shippingAddress.City = teslimatAdresi.City;
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = teslimatAdresi.OpenAddress;
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            var billingAddress = new Iyzipay.Model.Address();
            billingAddress.ContactName = appuser.Name + " " + appuser.Surname;
            billingAddress.City = teslimatAdresi.City;
            billingAddress.Country = "Turkey";
            billingAddress.Description = teslimatAdresi.OpenAddress;
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();


            //BasketItem firstBasketItem = new BasketItem();
            //firstBasketItem.Id = "BI101";
            //firstBasketItem.Name = "Binocular";
            //firstBasketItem.Category1 = "Collectibles";
            //firstBasketItem.Category2 = "Accessories";
            //firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            //firstBasketItem.Price = "0.3";
            //basketItems.Add(firstBasketItem);

            foreach (var item in cart.CartLines)
            {
                siparis.OrderLines.Add(new OrderLine
                {
                    ProductID = item.Product.ID,
                    OrderID = siparis.ID,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price


                });

                basketItems.Add(new BasketItem
                {
                    Id = item.Product.ID.ToString() ,
                    Name =item.Product.Name,
                    Category1 = "Category",
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Price = (item.Product.Price * item.Quantity).ToString().Replace(",",".")

                });
            }



            request.BasketItems = basketItems;

            Payment payment = await Payment.Create(request, options);
            #endregion

            try
            {
                await _serviceOrder.AddAsync(siparis);
                var sonuc = await _serviceOrder.SaveChangesAsync();
                if (sonuc > 0)
                {
                    HttpContext.Session.Remove("Cart");
                    return RedirectToAction("Thanks");
                }
            }
            catch (Exception)
            {

                TempData["Message"] = "HATA OLUŞTU!";
            }

            return View(model);
        }

        public IActionResult Thanks()
        {
            
            return View();
        }

        private CartService GetCart()
        {
            return HttpContext.Session.GetJson<CartService>("Cart") ?? new CartService();
        }
    }
}
