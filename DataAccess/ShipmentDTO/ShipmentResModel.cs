using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.TrackingInfoDTO;

namespace DataAccess.ShipmentDTO
{
    public class ShipmentResModel
    {
        public int ShipmentId { get; set; }
        public string TrackingNumber { get; set; } = null!;
        public string Carrier { get; set; } = null!;
        public decimal ShippingCost { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = null!;
        public DateTime EstimatedDeliveryDate { get; set; }
        public List<TrackingInfoResModel> TrackingHistory { get; set; } = new();
    }
}
