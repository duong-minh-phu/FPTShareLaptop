using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Enums;

namespace DataAccess.DonationFormDTO
{
    public class CreateDonationFormReqModel
    {

        public string ItemName { get; set; } = string.Empty; 

        public string? ItemDescription { get; set; } 

        public int Quantity { get; set; }  
      
    }
}
