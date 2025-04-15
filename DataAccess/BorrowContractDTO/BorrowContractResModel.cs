using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.BorrowContractDTO
{
    public class BorrowContractResponseModel
    {
        public int ContractId { get; set; }
        public int RequestId { get; set; }
        public int ItemId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
        public DateTime ContractDate { get; set; }
        public string Terms { get; set; }
        public string ConditionBorrow { get; set; }
        public decimal ItemValue { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public List<string> ContractImages { get; set; } = new List<string>();
    }
}
