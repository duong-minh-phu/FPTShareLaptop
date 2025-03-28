using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.WalletDTO
{
    public class WalletReqModel
    {
        public decimal Amount { get; set; } 
        public string Type { get; set; } = null!; 
        public string Status { get; set; } 
    }
}
