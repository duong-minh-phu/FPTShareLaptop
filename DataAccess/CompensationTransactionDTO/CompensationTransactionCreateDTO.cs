using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CompensationTransactionDTO
{
    public class CompensationTransactionCreateDTO
    {
        [Required]
        public int ContractId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ReportDamageId { get; set; }

        [Required]
        public int DepositTransactionId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Compensation amount must be non-negative.")]
        public decimal CompensationAmount { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Used deposit amount must be non-negative.")]
        public decimal UsedDepositAmount { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Extra payment required must be non-negative.")]
        public decimal ExtraPaymentRequired { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = null!;
    }
}
