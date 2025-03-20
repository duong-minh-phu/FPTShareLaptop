using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.BorrowContractDTO
{
    public class CreateBorrowContractReqModel
    {
        public int RequestId { get; set; }
        public int ItemId { get; set; }
        public string Terms { get; set; }
        public string ConditionBorrow { get; set; }
        public decimal ItemValue { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
    }
}
