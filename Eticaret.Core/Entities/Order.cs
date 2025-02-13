using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eticaret.Core.Entities
{
    public class Order : IEntity
    {
        public int ID { get; set; }
        [Display(Name ="Sipariş No"),StringLength(100)]
        public string OrderNumber { get; set; }
        [Display(Name = "Sipariş Toplamı")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Müşteri No")]
        public int AppUserID { get; set; }

        [Display(Name = "Müşteri"), StringLength(100)]
        public string CustomerID { get; set; }

        [Display(Name = "Fatura Adresi"), StringLength(100)]
        public string BillingAddress { get; set; }

        [Display(Name = "Teslimat Adresi"), StringLength(100)]

        public string DeliveryAddress { get; set; }

        [Display(Name = "Sipariş Tarihi")]

        public DateTime OrderDate { get; set; }

        public List<OrderLine>? OrderLines { get; set; }

        [Display(Name = "Müşteri")]
        public AppUser? AppUser { get; set; }

        [Display(Name = "Sipariş Durumu")]
        public EnumOrderState OrderState { get; set; }

    }

    public enum EnumOrderState
    {
        [Display(Name = "Onay Bekliyor")]
        Waiting,
        [Display(Name = "Onaylandı")]
        Approved,
        [Display(Name = "Kargoya Verildi")]
        Shipped,
        [Display(Name = "Tamamlandı")] 
        Completed,
        [Display(Name = "İptal Edildi")] 
        Cancelled,
        [Display(Name = "İade Edildi")] 
        Returned,
    }
}
