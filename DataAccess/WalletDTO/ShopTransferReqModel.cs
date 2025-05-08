using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.WalletDTO
{
    public class ShopTransferReqModel
    {
        public int ShopId { get; set; }
        public decimal Amount { get; set; }
        public int OrderId { get; set; }
    }
}
