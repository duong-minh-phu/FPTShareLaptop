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

        [Required]
        public string ScreenSize { get; set; } = null!;

        [Required]
        public string Storage { get; set; } = null!;

        [Required]
        public string Ram { get; set; } = null!;

        [Required]
        public string Cpu { get; set; } = null!;

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int ShopId { get; set; }

        public IFormFile? ImageFile { get; set; } // Ảnh tải lên Cloudinary
    }
}
