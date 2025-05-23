﻿using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IUnitOfWork : IDisposable
    {        
        IGenericRepository<BorrowHistory> BorrowHistory { get; }
        IGenericRepository<BorrowRequest> BorrowRequest { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<Category> Category { get; }
        IGenericRepository<DonateForm> DonateForm { get; }
        IGenericRepository<FeedbackBorrow> FeedbackBorrow { get; }
        IGenericRepository<Order> Order { get; }
        IGenericRepository<OrderDetail> OrderDetail { get; }
        IGenericRepository<Product> Product { get; }
        IGenericRepository<ProductImage> ProductImage { get; }
        IGenericRepository<Role> Role { get; }
        IGenericRepository<Shop> Shop { get; }        
        IGenericRepository<DonateItem> DonateItem { get; }
        IGenericRepository<ItemImage> ItemImage { get; }
        IGenericRepository<Student> Student { get; }
        IGenericRepository<User> Users { get; }
        IGenericRepository<ReportDamage> ReportDamage { get; }
        IGenericRepository<CompensationTransaction> CompensationTransaction { get; }
        IGenericRepository<DepositTransaction> DepositTransaction { get; }
        IGenericRepository<RefreshToken> RefreshToken { get; }
        IGenericRepository<ItemCondition> ItemCondition { get; }
        IGenericRepository<BorrowContract> BorrowContract { get; }

        IGenericRepository<Wallet> Wallet { get; }
        IGenericRepository<WalletTransaction> WalletTransaction { get; }
        IGenericRepository<RefundTransaction> RefundTransaction { get; }
        IGenericRepository<Payment> Payment { get; }
        IGenericRepository<PaymentMethod> PaymentMethod { get; }
        IGenericRepository<Shipment> Shipment { get; }
        IGenericRepository<TrackingInfo> TrackingInfo { get; }

        IGenericRepository<Major> Major { get; }
        IGenericRepository<SettlementTransaction> SettlementTransaction { get; }
        IGenericRepository<FeedbackProduct> FeedbackProduct { get; }
        IGenericRepository<PurchasedLaptop> PurchasedLaptop { get; }
        IGenericRepository<SponsorFund> SponsorFund { get; }
        IGenericRepository<TransactionLog> TransactionLog { get; }

        Task<int> SaveAsync();
    }
}
