using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.RefundTransactionDTO
{
    public class RefundTransactionResModel
    {
        public int RefundId { get; set; }
        public int OrderId { get; set; }
        public int PaymentId { get; set; }
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}
