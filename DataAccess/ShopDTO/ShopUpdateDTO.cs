using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ShopDTO
{
    public class ShopUpdateDTO
    {
        public string ShopName { get; set; } = null!;
        public string ShopAddress { get; set; } = null!;
        public string ShopPhone { get; set; } = null!;
        public string BusinessLicense { get; set; } = null!;
        public string BankName { get; set; } = null!;
        public string BankNumber { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
