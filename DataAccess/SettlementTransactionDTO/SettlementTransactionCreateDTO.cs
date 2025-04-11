using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SettlementTransactionDTO
{
    public class SettlementTransactionCreateDTO
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ShopWalletId { get; set; }

        [Required]
        public int ManagerWalletId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Fee cannot be negative.")]
        public decimal Fee { get; set; }

    }
}
