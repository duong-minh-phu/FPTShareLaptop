using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ItemConditionDTO
{
    public class UpdateItemConditionReqModel
    {
        public string ConditionType { get; set; }
        public string Description { get; set; } 
        public string ImageUrl { get; set; } 
        public string Status { get; set; } 
    }

}
