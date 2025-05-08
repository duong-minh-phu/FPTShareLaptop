using System;
using System.Collections.Generic;

namespace BusinessObjects.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Dob { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public string Avatar { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<BorrowContract> BorrowContracts { get; set; } = new List<BorrowContract>();

    public virtual ICollection<BorrowHistory> BorrowHistories { get; set; } = new List<BorrowHistory>();

    public virtual ICollection<BorrowRequest> BorrowRequests { get; set; } = new List<BorrowRequest>();

    public virtual ICollection<CompensationTransaction> CompensationTransactions { get; set; } = new List<CompensationTransaction>();

    public virtual ICollection<DepositTransaction> DepositTransactions { get; set; } = new List<DepositTransaction>();

    public virtual ICollection<DonateForm> DonateForms { get; set; } = new List<DonateForm>();

    public virtual ICollection<DonateItem> DonateItems { get; set; } = new List<DonateItem>();

    public virtual ICollection<FeedbackBorrow> FeedbackBorrows { get; set; } = new List<FeedbackBorrow>();

    public virtual ICollection<FeedbackProduct> FeedbackProducts { get; set; } = new List<FeedbackProduct>();

    public virtual ICollection<ItemCondition> ItemConditions { get; set; } = new List<ItemCondition>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual Role Role { get; set; } = null!;

    public virtual Shop? Shop { get; set; }

    public virtual ICollection<SponsorFund> SponsorFunds { get; set; } = new List<SponsorFund>();

    public virtual Student? Student { get; set; }

    public virtual ICollection<TransactionLog> TransactionLogs { get; set; } = new List<TransactionLog>();

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
