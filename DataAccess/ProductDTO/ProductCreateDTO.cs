using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ProductDTO
{
    public class ProductCreateDTO
    {
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ScreenSize { get; set; } = null!;
        public string Storage { get; set; } = null!;
        public string Ram { get; set; } = null!;
        public string Cpu { get; set; } = null!;
        public int CategoryId { get; set; }
        public int ShopId { get; set; }
    }
}
