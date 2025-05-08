using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class Wallet
{
    public int WalletId { get; set; }

    public int UserId { get; set; }

    public decimal Balance { get; set; }

    public string Type { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public int? ShopId { get; set; }

    public virtual ICollection<SettlementTransaction> SettlementTransactionManagerWallets { get; set; } = new List<SettlementTransaction>();

    public virtual ICollection<SettlementTransaction> SettlementTransactionShopWallets { get; set; } = new List<SettlementTransaction>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<WalletTransaction> WalletTransactions { get; set; } = new List<WalletTransaction>();
}
