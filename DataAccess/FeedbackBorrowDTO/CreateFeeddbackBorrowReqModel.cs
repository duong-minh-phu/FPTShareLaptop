using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Enums;

namespace DataAccess.FeedbackBorrowDTO
{
    public class CreateFeedbackBorrowReqModel
    {
        [Required(ErrorMessage = "BorrowHistoryId is required.")]
        public int BorrowHistoryId { get; set; }
        [Required(ErrorMessage = "ItemId is required.")]
        public int ItemId { get; set; }
        [Required(ErrorMessage = "Rating is required.")]
        public int Rating { get; set; }
        [Required(ErrorMessage = "Comments is required.")]
        public string Comments { get; set; }
     
    }
}
