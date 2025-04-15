using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PurchasedLaptopDTO
{
    public class PurchasedLaptopReadDTO
    {
        public int PurchasedLaptopId { get; set; }
        public int ItemId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime PurchasedDate { get; set; }
        public string InvoiceImageUrl { get; set; } = null!;
    }
}
