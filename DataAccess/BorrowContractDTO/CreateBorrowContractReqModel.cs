using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.BorrowContractDTO
{
    public class CreateBorrowContractReqModel
    {
        [Required(ErrorMessage = "RequestId is required.")]
        public int RequestId { get; set; }

        [Required(ErrorMessage = "ItemId is required.")]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Terms are required.")]
        [StringLength(500, ErrorMessage = "Terms cannot be longer than 500 characters.")]
        public string Terms { get; set; }

        [Required(ErrorMessage = "ConditionBorrow is required.")]
        [StringLength(500, ErrorMessage = "ConditionBorrow cannot be longer than 500 characters.")]
        public string ConditionBorrow { get; set; }

        [Required(ErrorMessage = "ItemValue is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "ItemValue must be greater than zero.")]
        public decimal ItemValue { get; set; }

        [Required(ErrorMessage = "ExpectedReturnDate is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime ExpectedReturnDate { get; set; }
    }
}

