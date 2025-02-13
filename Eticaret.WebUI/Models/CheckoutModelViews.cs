﻿using Eticaret.Core.Entities;

namespace Eticaret.WebUI.Models
{
    public class CheckoutModelViews
    {
        public List<CartLine>? CartProducts { get; set; }
        public decimal TotalPrice { get; set; }

        public List<Address>? Addresses { get; set; }
    }
}
