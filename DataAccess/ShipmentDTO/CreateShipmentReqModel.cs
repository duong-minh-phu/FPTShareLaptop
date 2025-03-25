using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ShipmentDTO
{
    public class CreateShipmentReqModel
    {
        public int OrderId { get; set; }
        public string TrackingNumber { get; set; } = null!;
        public DateTime EstimatedDeliveryDate { get; set; }
        public string Carrier { get; set; } = null!;
        public decimal ShippingCost { get; set; }
    }
}
