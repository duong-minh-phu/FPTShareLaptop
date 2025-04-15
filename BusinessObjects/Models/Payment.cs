﻿using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public int PaymentMethodId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string TransactionCode { get; set; }

    public string Status { get; set; }

    public virtual Order Order { get; set; }

    public virtual PaymentMethod PaymentMethod { get; set; }

    public virtual ICollection<RefundTransaction> RefundTransactions { get; set; } = new List<RefundTransaction>();
}
