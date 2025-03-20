using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class RefundTransaction
{
    public int RefundId { get; set; }

    public int OrderId { get; set; }

    public int PaymentId { get; set; }

    public int WalletId { get; set; }

    public decimal Amount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Payment Payment { get; set; } = null!;

    public virtual Wallet Wallet { get; set; } = null!;
}
