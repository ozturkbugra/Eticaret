using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eticaret.Core.Entities;
using Eticaret.Service.Abstract;

namespace Eticaret.Service.Concrete
{
    public class CartService : ICartService
    {
        public List<CartLine> CartLines = new();
        public void AddProduct(Product product, int quantity)
        {
            var urun = CartLines.FirstOrDefault(x=> x.Product.ID == product.ID);
            if (urun != null) {
                urun.Quantity += quantity;
            }
            else
            {
                CartLines.Add(new CartLine
                {
                    Quantity = quantity,
                    Product = product,
                });
            }
        }

        public void ClearAll()
        {
           CartLines.Clear();
        }

        public void RemoveProduct(Product product)
        {
            CartLines.RemoveAll(x=> x.Product.ID == product.ID);
        }

        public decimal ToTalPrice()
        {
            return CartLines.Sum(x=> x.Product.Price * x.Quantity);
        }

        public void UpdateProduct(Product product, int quantity)
        {
            var urun = CartLines.FirstOrDefault(x => x.Product.ID == product.ID);
            if (urun != null)
            {
                urun.Quantity = quantity;
            }
            else
            {
                CartLines.Add(new CartLine
                {
                    Quantity = quantity,
                    Product = product,
                });
            }
        }
    }
}
