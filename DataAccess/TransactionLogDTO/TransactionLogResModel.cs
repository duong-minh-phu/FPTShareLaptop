using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.TransactionLogDTO
{
    public class TransactionLogResModel
    {
        public int TransactionId { get; set; }

        public int UserId { get; set; }

        public string TransactionType { get; set; } = null!;

        public decimal Amount { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? Note { get; set; }

        public int? ReferenceId { get; set; }

        public string? SourceTable { get; set; }

        public decimal? UsedDepositAmount { get; set; }

        public decimal? ExtraPaymentRequired { get; set; }
    }
}
