using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SettlementTransactionDTO
{
    public class SettlementTransactionDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ShopWalletId { get; set; }
        public int ManagerWalletId { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
