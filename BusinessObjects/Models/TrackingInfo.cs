﻿using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class TrackingInfo
{
    public int TrackingId { get; set; }

    public int OrderId { get; set; }

    public int ShipmentId { get; set; }

    public string Status { get; set; }

    public string Location { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Order Order { get; set; }

    public virtual Shipment Shipment { get; set; }
}
