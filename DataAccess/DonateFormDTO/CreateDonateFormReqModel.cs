using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Enums;

namespace DataAccess.DonationFormDTO
{
    public class CreateDonateFormReqModel
    {
        [Required(ErrorMessage = "ItemName is required")]
        public string ItemName { get; set; } = string.Empty;
        [Required(ErrorMessage = "ItemDescription is required")]
        public string ItemDescription { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }  
      
    }
}
