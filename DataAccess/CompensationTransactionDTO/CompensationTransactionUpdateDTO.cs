using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CompensationTransactionDTO
{
    public class CompensationTransactionUpdateDTO
    {
        [Required]
        public int CompensationId { get; set; }

        public int? ContractId { get; set; }
        public int? UserId { get; set; }
        public int? ReportDamageId { get; set; }
        public int? DepositTransactionId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Compensation amount must be non-negative.")]
        public decimal? CompensationAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Used deposit amount must be non-negative.")]
        public decimal? UsedDepositAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Extra payment required must be non-negative.")]
        public decimal? ExtraPaymentRequired { get; set; }

        [MaxLength(50)]
        public string? Status { get; set; }
    }
}
