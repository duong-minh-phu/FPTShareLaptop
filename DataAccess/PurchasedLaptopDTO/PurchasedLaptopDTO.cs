using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PurchasedLaptopDTO
{
    public class PurchasedLaptopDTO
    {
        public int PurchasedLaptopId { get; set; }
        public int DonateItemId { get; set; }
        public int SponsorFundId { get; set; }
        public decimal PurchaseAmount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Status { get; set; } = null!;
        public string? PurchasedImageUrl { get; set; }
    }
}
