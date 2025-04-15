using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ProductDTO
{
    public class ProductReadDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ImageProduct { get; set; } = null!;
        public string ScreenSize { get; set; } = null!;
        public string Storage { get; set; } = null!;
        public string Ram { get; set; } = null!;
        public string Cpu { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string GraphicsCard { get; set; } = null!;
        public string Battery { get; set; } = null!;
        public string Ports { get; set; } = null!;
        public int ProductionYear { get; set; }
        public string OperatingSystem { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int CategoryId { get; set; }
        public int ShopId { get; set; }

        public string CategoryName { get; set; } = null!;
        public string ShopName { get; set; } = null!;
    }
}
