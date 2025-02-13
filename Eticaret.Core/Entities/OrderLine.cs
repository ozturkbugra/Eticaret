using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eticaret.Core.Entities
{
    public class OrderLine : IEntity
    {
        public int ID { get; set; }

        [Display(Name = "Sipariş No")]
        public int OrderID { get; set; }

        [Display(Name = "Sipariş")]
        public Order? Order { get; set; }

        [Display(Name = "Ürün No")]
        public int ProductID  { get; set; }

        [Display(Name = "Ürün")]
        public Product? Product { get; set; }

        [Display(Name = "Miktar")]
        public int Quantity { get; set; }

        [Display(Name = "Birim Fiyat")]
        public decimal UnitPrice { get; set; }
    }
}
