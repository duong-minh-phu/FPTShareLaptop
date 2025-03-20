using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Enums;

namespace DataAccess.BorrowContractDTO
{
    public class UpdateBorrowContractReqModel
    {
        public string Status { get; set; }
        public string Terms { get; set; }
        public string ConditionBorrow { get; set; }
        public decimal? ItemValue { get; set; }
        public DateTime? ExpectedReturnDate { get; set; }
    }
}
