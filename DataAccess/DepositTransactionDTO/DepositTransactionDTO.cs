using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DepositTransactionDTO
{
    public class DepositTransactionDTO
    {
        public int DepositId { get; set; }
        public int? ContractId { get; set; }
        public int? UserId { get; set; }
        public string? Status { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? DepositDate { get; set; }
    }
}
