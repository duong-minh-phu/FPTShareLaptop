using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ShopDTO
{
    public class ShopReadDTO
    {
        public int ShopId { get; set; }
        public int UserId { get; set; }
        public string ShopName { get; set; }
        public string ShopAddress { get; set; }
        public string ShopPhone { get; set; }
        public string BusinessLicense { get; set; }
        public string BankName { get; set; }
        public string BankNumber { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
