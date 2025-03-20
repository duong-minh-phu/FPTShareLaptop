using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Enums;

namespace DataAccess.FeedbackBorrowDTO
{
    public class UpdateFeedbackBorrowReqModel
    {
        public int Rating { get; set; } 
        public string Comments { get; set; } 
        public bool IsAnonymous { get; set; } 
    }
}
