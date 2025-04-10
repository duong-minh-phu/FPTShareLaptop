using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Enums;

namespace DataAccess.DonationFormDTO
{
    public class DonateFormResModel
    {
        public int DonationFormId { get; set; }

        public int SponsorId { get; set; }

        public string ItemName { get; set; } = string.Empty;
        
        public string ItemDescription { get; set; }

        public string? ImageDonateForm { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Status { get; set; }
    }
}
