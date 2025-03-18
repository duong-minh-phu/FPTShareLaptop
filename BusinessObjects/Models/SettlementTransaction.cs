﻿using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class SettlementTransaction
{
    public int SettlementId { get; set; }

    public int? OrderId { get; set; }

    public int? ShopWalletId { get; set; }

    public int? ManagerWalletId { get; set; }

    public decimal? Amount { get; set; }

    public decimal? Fee { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Wallet? ManagerWallet { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Wallet? ShopWallet { get; set; }
}
