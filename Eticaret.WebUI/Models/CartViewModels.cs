using Eticaret.Core.Entities;

namespace Eticaret.WebUI.Models
{
    public class CartViewModels
    {
        public List<CartLine>? CartLines { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
