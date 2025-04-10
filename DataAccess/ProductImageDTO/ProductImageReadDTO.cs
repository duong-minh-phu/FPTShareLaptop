using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ProductImageDTO
{
    public class ProductImageReadDTO
    {
        public int ProductImageId { get; set; }
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
