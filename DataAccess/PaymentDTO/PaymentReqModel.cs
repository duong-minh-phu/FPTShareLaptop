using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PaymentDTO
{
    public class PaymentReqModel
    {
        public int PaymentMethodId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionCode { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public string Note { get; set; }
    }
}
