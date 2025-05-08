using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PurchasedLaptopDTO
{
    public class PurchasedLaptopCreateDTO
    {
        public int DonateItemId { get; set; }
        public int SponsorFundId { get; set; }
        public decimal PurchaseAmount { get; set; }
        
        
    }
}
