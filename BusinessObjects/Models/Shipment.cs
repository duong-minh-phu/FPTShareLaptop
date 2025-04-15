using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Shipment
{
    public int ShipmentId { get; set; }

    public int OrderId { get; set; }

    public string TrackingNumber { get; set; }

    public string Carrier { get; set; }

    public string Status { get; set; }

    public DateTime EstimatedDeliveryDate { get; set; }

    public decimal ShippingCost { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Order Order { get; set; }

    public virtual ICollection<TrackingInfo> TrackingInfos { get; set; } = new List<TrackingInfo>();
}
