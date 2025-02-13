using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Eticaret.Core.Entities
{
    public class Address : IEntity
    {
        public int ID { get; set; }
        [Display(Name = "Adres Başlığı" ),Required(ErrorMessage = "{0} Alanı Zorunludur!")]
        [StringLength(50)]
        public string Title { get; set; }

        [Display(Name = "Şehir")]
        [StringLength(100)]
        public string City { get; set; }

        [Display(Name = "İlçe")]
        [StringLength(100)]
        public string District { get; set; }
        
        [Display(Name = "Açık Adres"), DataType(DataType.MultilineText), Required(ErrorMessage = "{0} Alanı Zorunludur!")]
        [StringLength(50000)]
        public string OpenAddress { get; set; }

        [Display(Name = "Aktif")]

        public bool IsActive { get; set; }

        [Display(Name = "Fatura Adresi")]

        public bool IsBillingAddress { get; set; } // Fatura Adresi

        [Display(Name = "Teslimat Adresi"), Required(ErrorMessage = "{0} Alanı Zorunludur!")]

        public bool IsDeliveryAddress { get; set; } // Teslimat Adresi

        [Display(Name = "Kayıt Tarihi"), ScaffoldColumn(false)]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [ScaffoldColumn(false)]
        public Guid? AddressGuid { get; set; } = Guid.NewGuid();

        public int AppUserID { get; set; }
        public AppUser? AppUser { get; set; }



    }
}
