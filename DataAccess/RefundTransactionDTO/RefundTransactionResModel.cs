using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.RefundTransactionDTO
{
    public class RefundTransactionResModel
    {
        public int RefundTransactionId { get; set; }
        
        public int ContractId { get; set; }

        public int UserId { get; set; }

        public int DepositId { get; set; }
        
        public int ReportId { get; set; }

        public decimal RefundAmount { get; set; }

        public string? RefundNote { get; set; }

        public DateTime RefundDate { get; set; }

        public string Status { get; set; } = null!;
    }
}
