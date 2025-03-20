using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Shipment
{
    public int ShipmentId { get; set; }

    public int OrderId { get; set; }

    public string TrackingNumber { get; set; } = null!;

    public string Carrier { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime EstimatedDeliveryDate { get; set; }

    public decimal ShippingCost { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual ICollection<TrackingInfo> TrackingInfos { get; set; } = new List<TrackingInfo>();
}
