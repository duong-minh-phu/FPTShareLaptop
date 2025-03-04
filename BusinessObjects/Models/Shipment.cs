using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Shipment
{
    public int ShipmentId { get; set; }

    public int? OrderId { get; set; }

    public string? ShippingAddress { get; set; }

    public string? ShipperName { get; set; }

    public string? TrackingNumber { get; set; }

    public DateTime? EstimatedDeliveryDate { get; set; }

    public DateTime? ActualDeliveryDate { get; set; }

    public string? Status { get; set; }

    public virtual Order? Order { get; set; }
}
