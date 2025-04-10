using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DepositTransactionDTO
{
    public class DepositTransactionUpdateDTO
    {
        public int DepositId { get; set; } // Bắt buộc nhập khi cập nhật
        public int? ContractId { get; set; }
        public int? UserId { get; set; }
        public string? Status { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? DepositDate { get; set; }
    }
}
