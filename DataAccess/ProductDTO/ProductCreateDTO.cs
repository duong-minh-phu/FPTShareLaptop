using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ProductDTO
{
    public class ProductCreateDTO
    {
        [Required]
        public string ProductName { get; set; } = null!;

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public IFormFile? ImageFile { get; set; }

        [Required]
        public string ScreenSize { get; set; } = null!;

        [Required]
        public string Storage { get; set; } = null!;

        [Required]
        public string Ram { get; set; } = null!;

        [Required]
        public string Cpu { get; set; } = null!;

        [Required]
        public string Model { get; set; } = null!;

        [Required]
        public string Color { get; set; } = null!;

        [Required]
        public string GraphicsCard { get; set; } = null!;

        [Required]
        public string Battery { get; set; } = null!;

        [Required]
        public string Ports { get; set; } = null!;

        [Required]
        public int ProductionYear { get; set; }

        [Required]
        public string OperatingSystem { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int ShopId { get; set; }
    }
}
