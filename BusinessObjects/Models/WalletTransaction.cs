using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class WalletTransaction
{
    public int TransactionId { get; set; }

    public int WalletId { get; set; }

    public string TransactionType { get; set; } = null!;

    public decimal Amount { get; set; }

    public int RelatedPaymentId { get; set; }

    public DateTime CreatedDate { get; set; }

    public string Note { get; set; } = null!;

    public virtual Payment RelatedPayment { get; set; } = null!;

    public virtual Wallet Wallet { get; set; } = null!;
}
