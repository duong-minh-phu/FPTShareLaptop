using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ItemConditionDTO
{
    public class CreateItemConditionReqModel
    {
        public int ItemId { get; set; }
        public string ConditionType { get; set; } 
        public string Description { get; set; }  
        public string ImageUrl { get; set; }  
        public string CheckedBy { get; set; }  
    }

}
