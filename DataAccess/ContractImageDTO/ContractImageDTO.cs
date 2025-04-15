using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ContractImageDTO
{
    public class ContractImageDTO
    {
        public int ContractImageId { get; set; }
        public int BorrowContractId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}
