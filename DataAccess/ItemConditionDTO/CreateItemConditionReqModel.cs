using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ItemConditionDTO
{
    public class CreateItemConditionReqModel
    {
        [Required(ErrorMessage = "ItemId is required.")]
        public int ItemId { get; set; }
        [Required(ErrorMessage = "ConditionType is required.")]
        public string ConditionType { get; set; } 
        public string Description { get; set; }
        [Required(ErrorMessage = "ImageUrl is required.")]
        public string ImageUrl { get; set; }  
        public string CheckedBy { get; set; }  
    }

}
