using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class OrderDetail
{
    public int OrderItemId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? PriceItem { get; set; }

    public virtual ICollection<FeedbackProduct> FeedbackProducts { get; set; } = new List<FeedbackProduct>();

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
