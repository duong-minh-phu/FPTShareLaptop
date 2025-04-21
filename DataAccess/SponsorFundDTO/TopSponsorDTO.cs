using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SponsorFundDTO
{
    public class TopSponsorDTO
    {
        public int SponsorId { get; set; }
        public string SponsorName { get; set; } = null!;
        public decimal TotalAmount { get; set; }
    }
}
