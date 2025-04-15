﻿using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class FeedbackProduct
{
    public int FeedbackProductId { get; set; }

    public int OrderItemId { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public DateTime FeedbackDate { get; set; }

    public int Rating { get; set; }

    public string Comments { get; set; }

    public bool IsAnonymous { get; set; }

    public virtual OrderDetail OrderItem { get; set; }

    public virtual Product Product { get; set; }

    public virtual User User { get; set; }
}
