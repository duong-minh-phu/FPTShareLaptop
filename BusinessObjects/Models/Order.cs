﻿using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public decimal TotalPrice { get; set; }

    public string OrderAddress { get; set; } = null!;

    public string Field { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<SettlementTransaction> SettlementTransactions { get; set; } = new List<SettlementTransaction>();

    public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();

    public virtual ICollection<TrackingInfo> TrackingInfos { get; set; } = new List<TrackingInfo>();

    public virtual User User { get; set; } = null!;
}
