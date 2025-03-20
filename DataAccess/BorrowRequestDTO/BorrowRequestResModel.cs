using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Enums;

namespace DataAccess.BorrowRequestDTO
{
    public class BorrowRequestResModel
    {
        public int RequestId { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
