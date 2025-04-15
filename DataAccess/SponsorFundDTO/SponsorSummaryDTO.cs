using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SponsorFundDTO
{
    public class SponsorSummaryDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public decimal TotalDonated { get; set; }
        public decimal TotalContributed { get; set; }
        public decimal RemainingBalance => TotalDonated - TotalContributed;
    }
}
