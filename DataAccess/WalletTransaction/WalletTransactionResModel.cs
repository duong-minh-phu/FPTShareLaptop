using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.WalletTransaction
{
    public class WalletTransactionResModel
    {
        public int TransactionId { get; set; }
        public int WalletId { get; set; }
        public string TransactionType { get; set; } = null!;
        public decimal Amount { get; set; }
        public int RelatedPaymentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Note { get; set; } = null!;
    }
}
