using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SponsorFundDTO
{
    public class SponsorFundReadDTO
    {
        public int SponsorFundId { get; set; }
        public int SponsorId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime TransferDate { get; set; }
        public string? ProofImageUrl { get; set; }
        public string? Status { get; set; }
    }
}
