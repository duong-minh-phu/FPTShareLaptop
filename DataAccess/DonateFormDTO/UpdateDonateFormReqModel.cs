using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Http;

namespace DataAccess.DonationFormDTO
{
    public class UpdateDonateFormReqModel
    {
        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }

    }
}
