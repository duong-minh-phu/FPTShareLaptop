using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.UserDTO;

namespace DataAccess.PaymentDTO
{
    public class PaymentViewResModel
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public int PaymentMethodId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string TransactionCode { get; set; }

    }
}
