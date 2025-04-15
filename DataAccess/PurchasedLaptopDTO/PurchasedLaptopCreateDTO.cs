﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PurchasedLaptopDTO
{
    public class PurchasedLaptopCreateDTO
    {
        public int ItemId { get; set; }
        public decimal TotalPrice { get; set; }
        public string InvoiceImageUrl { get; set; } = null!;
    }
}
