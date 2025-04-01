using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ShipmentDTO
{
    public class UpdateShipmentReqModel
    {
        public string Status { get; set; } = null!;
        public string Carrier { get; set; } = null!;
        public decimal ShippingCost { get; set; }

    }
}
