using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ItemImageDTO
{
    public class ItemImageDTO
    {
        public int ItemImageId { get; set; }
        public int ItemId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
