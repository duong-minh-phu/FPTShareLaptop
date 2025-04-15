using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SponsorFundDTO
{
    public class SponsorFundCreateDTO
    {
        public int SponsorId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime TransferDate { get; set; }
    }
}
