using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SponsorFundDTO
{
    public class SponsorFundUpdateDTO
    {
        public decimal? Amount { get; set; }
        public string? Description { get; set; }
        public DateTime? TransferDate { get; set; }
        public string? Status { get; set; }
    }
}
