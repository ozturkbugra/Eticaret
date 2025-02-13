using Eticaret.Core.Entities;
using Eticaret.Data;
using Eticaret.Service.Abstract;
using Eticaret.WebUI.ExtensionMethod;
using Microsoft.AspNetCore.Mvc;

namespace Eticaret.WebUI.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IService<Product> _service;

        public FavoritesController(IService<Product> service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            var favoriler = GetFavorites();
            return View(favoriler);
        }

        private List<Product> GetFavorites() 
        {
            return HttpContext.Session.GetJson<List<Product>>("GetFavorites") ?? [];
        }

        public IActionResult Add(int ProductID)
        {
            var favoriler = GetFavorites();
            var product = _service.Find(ProductID);
            if (product != null && !favoriler.Any(x=> x.ID == ProductID)) 
            { 
                favoriler.Add(product);
                HttpContext.Session.SetJson("GetFavorites",favoriler);
                
            }
            return RedirectToAction("Index");
        }
        
        public IActionResult Remove(int ProductID)
        {
            var favoriler = GetFavorites();
            var product = _service.Find(ProductID);
            if (product != null && favoriler.Any(x=> x.ID == ProductID)) 
            { 
                favoriler.RemoveAll(x=> x.ID == product.ID);
                HttpContext.Session.SetJson("GetFavorites",favoriler);
                
            }
            return RedirectToAction("Index");
        }
    }

   
}
