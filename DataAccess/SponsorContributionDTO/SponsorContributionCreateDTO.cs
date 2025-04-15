using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SponsorContributionDTO
{
    public class SponsorContributionCreateDTO
    {
        public int SponsorFundId { get; set; }
        public int PurchasedLaptopId { get; set; }
        public decimal ContributedAmount { get; set; }
    }
}
