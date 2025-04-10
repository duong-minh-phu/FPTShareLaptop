using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CompensationTransactionDTO
{
    public class CompensationTransactionDTO
    {
        public int? CompensationId { get; set; }
        public int? ContractId { get; set; }
        public int? UserId { get; set; }
        public int? ReportDamageId { get; set; }
        public int? DepositTransactionId { get; set; }
        public decimal? CompensationAmount { get; set; }
        public decimal? UsedDepositAmount { get; set; }
        public decimal? ExtraPaymentRequired { get; set; }
        public string? Status { get; set; }
    }
}
