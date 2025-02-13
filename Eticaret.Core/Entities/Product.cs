using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eticaret.Core.Entities
{
    public class Product : IEntity
    {
        public int ID { get; set; }

        [Display(Name = "Adı")]
        public string Name { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Resim")]
        public string? Image { get; set; }

        [Display(Name = "Fiyat")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Price { get; set; }

        [Display(Name = "Ürün Kodu")]
        public string? ProductCode { get; set; }

        [Display(Name = "Stok Miktarı")]
        public int Stock { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; }

        [Display(Name = "Ana Sayfada Göster")]
        public bool IsHome { get; set; }

      
        public int? CategoryID { get; set; }
        [Display(Name = "Kategori")]
        public Category? Category { get; set; }

        
        public int? BrandID { get; set; }
        [Display(Name = "Marka")]
        public Brand? Brand { get; set; }

        [Display(Name = "Sıra No")]
        public int OrderNo { get; set; }
        [Display(Name = "Kayıt Tarihi"), ScaffoldColumn(false)]
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
