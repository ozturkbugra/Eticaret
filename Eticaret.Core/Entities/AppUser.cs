using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eticaret.Core.Entities
{
    public class AppUser : IEntity
    {
        public int ID { get; set; }

        [Display(Name= "Adı")]
        public string Name { get; set; }

        [Display(Name = "Soyadı")]
        public string Surname { get; set; }
        public string Email { get; set; }

        [Display(Name = "Telefon")]
        public string? Phone { get; set; }

        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string? Username { get; set; } = string.Empty;

        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; }

        [Display(Name = "Admin Mi?")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Kayıt Tarihi"), ScaffoldColumn(false)]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [ScaffoldColumn(false)]
        public Guid? UserGuid { get; set; } = Guid.NewGuid();

        public List<Address>? Addresses { get; set; }


    }
}
