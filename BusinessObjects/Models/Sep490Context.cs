using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessObjects.Models;

public partial class Sep490Context : DbContext
{
    public Sep490Context()
    {
    }

    public Sep490Context(DbContextOptions<Sep490Context> options)
        : base(options)
    {
    }

    public virtual DbSet<BorrowContract> BorrowContracts { get; set; }

    public virtual DbSet<BorrowHistory> BorrowHistories { get; set; }

    public virtual DbSet<BorrowRequest> BorrowRequests { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CompensationTransaction> CompensationTransactions { get; set; }

    public virtual DbSet<DepositTransaction> DepositTransactions { get; set; }

    public virtual DbSet<DonateForm> DonateForms { get; set; }

    public virtual DbSet<DonateItem> DonateItems { get; set; }

    public virtual DbSet<FeedbackBorrow> FeedbackBorrows { get; set; }

    public virtual DbSet<FeedbackProduct> FeedbackProducts { get; set; }

    public virtual DbSet<ItemCondition> ItemConditions { get; set; }

    public virtual DbSet<ItemImage> ItemImages { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<RefundTransaction> RefundTransactions { get; set; }

    public virtual DbSet<ReportDamage> ReportDamages { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SettlementTransaction> SettlementTransactions { get; set; }

    public virtual DbSet<Shipment> Shipments { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<TrackingInfo> TrackingInfos { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    public virtual DbSet<WalletTransaction> WalletTransactions { get; set; }

    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
        return configuration["ConnectionStrings:DefaultConnectionString"];
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(GetConnectionString());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BorrowContract>(entity =>
        {
            entity.HasKey(e => e.ContractId).HasName("PK__BorrowCo__C90D34692526AC4C");

            entity.ToTable("BorrowContract");

            entity.Property(e => e.ConditionBorrow).HasMaxLength(255);
            entity.Property(e => e.ContractDate).HasColumnType("datetime");
            entity.Property(e => e.ExpectedReturnDate).HasColumnType("datetime");
            entity.Property(e => e.ItemValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Terms).HasMaxLength(255);

            entity.HasOne(d => d.Item).WithMany(p => p.BorrowContracts)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__BorrowCon__ItemI__5DCAEF64");

            entity.HasOne(d => d.Request).WithMany(p => p.BorrowContracts)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("FK__BorrowCon__Reque__5CD6CB2B");

            entity.HasOne(d => d.User).WithMany(p => p.BorrowContracts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__BorrowCon__UserI__5EBF139D");
        });

        modelBuilder.Entity<BorrowHistory>(entity =>
        {
            entity.HasKey(e => e.BorrowHistoryId).HasName("PK__BorrowHi__1F7C51B59DFD7D5B");

            entity.ToTable("BorrowHistory");

            entity.Property(e => e.BorrowDate).HasColumnType("datetime");
            entity.Property(e => e.ReturnDate).HasColumnType("datetime");

            entity.HasOne(d => d.Item).WithMany(p => p.BorrowHistories)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__BorrowHis__ItemI__66603565");

            entity.HasOne(d => d.Request).WithMany(p => p.BorrowHistories)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("FK__BorrowHis__Reque__656C112C");

            entity.HasOne(d => d.User).WithMany(p => p.BorrowHistories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__BorrowHis__UserI__6754599E");
        });

        modelBuilder.Entity<BorrowRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__BorrowRe__33A8517AECF6D4E7");

            entity.ToTable("BorrowRequest");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Item).WithMany(p => p.BorrowRequests)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__BorrowReq__ItemI__59FA5E80");

            entity.HasOne(d => d.User).WithMany(p => p.BorrowRequests)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__BorrowReq__UserI__59063A47");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B09B92009");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryName).HasMaxLength(255);
        });

        modelBuilder.Entity<CompensationTransaction>(entity =>
        {
            entity.HasKey(e => e.CompensationId).HasName("PK__Compensa__14AB97598997E2C5");

            entity.ToTable("CompensationTransaction");

            entity.Property(e => e.CompensationAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ExtraPaymentRequired).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UsedDepositAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Contract).WithMany(p => p.CompensationTransactions)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK__Compensat__Contr__6E01572D");

            entity.HasOne(d => d.DepositTransaction).WithMany(p => p.CompensationTransactions)
                .HasForeignKey(d => d.DepositTransactionId)
                .HasConstraintName("FK__Compensat__Depos__70DDC3D8");

            entity.HasOne(d => d.ReportDamage).WithMany(p => p.CompensationTransactions)
                .HasForeignKey(d => d.ReportDamageId)
                .HasConstraintName("FK__Compensat__Repor__6FE99F9F");

            entity.HasOne(d => d.User).WithMany(p => p.CompensationTransactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Compensat__UserI__6EF57B66");
        });

        modelBuilder.Entity<DepositTransaction>(entity =>
        {
            entity.HasKey(e => e.DepositId).HasName("PK__DepositT__AB60DF7142BE6EBF");

            entity.ToTable("DepositTransaction");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DepositDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Contract).WithMany(p => p.DepositTransactions)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK__DepositTr__Contr__619B8048");

            entity.HasOne(d => d.User).WithMany(p => p.DepositTransactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__DepositTr__UserI__628FA481");
        });

        modelBuilder.Entity<DonateForm>(entity =>
        {
            entity.HasKey(e => e.DonateFormId).HasName("PK__DonateFo__4F919634ED4CE3C1");

            entity.ToTable("DonateForm");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DonateQuantity).HasDefaultValue(1);
            entity.Property(e => e.ItemDescription).HasMaxLength(255);
            entity.Property(e => e.ItemName).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.DonateForms)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__DonateFor__UserI__4F7CD00D");
        });

        modelBuilder.Entity<DonateItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__DonateIt__727E838B415B73C2");

            entity.ToTable("DonateItem");

            entity.Property(e => e.ConditionItem).HasMaxLength(255);
            entity.Property(e => e.Cpu)
                .HasMaxLength(50)
                .HasColumnName("CPU");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ItemImage).HasMaxLength(255);
            entity.Property(e => e.ItemName).HasMaxLength(255);
            entity.Property(e => e.Ram)
                .HasMaxLength(50)
                .HasColumnName("RAM");
            entity.Property(e => e.ScreenSize).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Storage).HasMaxLength(50);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.DonateForm).WithMany(p => p.DonateItems)
                .HasForeignKey(d => d.DonateFormId)
                .HasConstraintName("FK__DonateIte__Donat__534D60F1");

            entity.HasOne(d => d.User).WithMany(p => p.DonateItems)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__DonateIte__UserI__52593CB8");
        });

        modelBuilder.Entity<FeedbackBorrow>(entity =>
        {
            entity.HasKey(e => e.FeedbackBorrowId).HasName("PK__Feedback__ACD684AD68E3AE49");

            entity.ToTable("FeedbackBorrow");

            entity.Property(e => e.Comments).HasMaxLength(255);
            entity.Property(e => e.FeedbackDate).HasColumnType("datetime");

            entity.HasOne(d => d.BorrowHistory).WithMany(p => p.FeedbackBorrows)
                .HasForeignKey(d => d.BorrowHistoryId)
                .HasConstraintName("FK__FeedbackB__Borro__7A672E12");

            entity.HasOne(d => d.Item).WithMany(p => p.FeedbackBorrows)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__FeedbackB__ItemI__7B5B524B");

            entity.HasOne(d => d.User).WithMany(p => p.FeedbackBorrows)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__FeedbackB__UserI__7C4F7684");
        });

        modelBuilder.Entity<FeedbackProduct>(entity =>
        {
            entity.HasKey(e => e.FeedbackProductId).HasName("PK__Feedback__7C6D4B9C3A13E627");

            entity.ToTable("FeedbackProduct");

            entity.Property(e => e.Comments).HasMaxLength(255);
            entity.Property(e => e.FeedbackDate).HasColumnType("datetime");

            entity.HasOne(d => d.OrderItem).WithMany(p => p.FeedbackProducts)
                .HasForeignKey(d => d.OrderItemId)
                .HasConstraintName("FK__FeedbackP__Order__1BC821DD");

            entity.HasOne(d => d.Product).WithMany(p => p.FeedbackProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__FeedbackP__Produ__1CBC4616");

            entity.HasOne(d => d.User).WithMany(p => p.FeedbackProducts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__FeedbackP__UserI__1DB06A4F");
        });

        modelBuilder.Entity<ItemCondition>(entity =>
        {
            entity.HasKey(e => e.ConditionId).HasName("PK__ItemCond__37F5C0CF8D5C11A8");

            entity.ToTable("ItemCondition");

            entity.Property(e => e.CheckedBy).HasMaxLength(255);
            entity.Property(e => e.CheckedDate).HasColumnType("datetime");
            entity.Property(e => e.ConditionType).HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("ImageURL");
            entity.Property(e => e.RefundDate).HasColumnType("datetime");

            entity.HasOne(d => d.BorrowHistory).WithMany(p => p.ItemConditions)
                .HasForeignKey(d => d.BorrowHistoryId)
                .HasConstraintName("FK__ItemCondi__Borro__76969D2E");

            entity.HasOne(d => d.Contract).WithMany(p => p.ItemConditions)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("FK__ItemCondi__Contr__74AE54BC");

            entity.HasOne(d => d.Item).WithMany(p => p.ItemConditions)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__ItemCondi__ItemI__73BA3083");

            entity.HasOne(d => d.Report).WithMany(p => p.ItemConditions)
                .HasForeignKey(d => d.ReportId)
                .HasConstraintName("FK__ItemCondi__Repor__778AC167");

            entity.HasOne(d => d.User).WithMany(p => p.ItemConditions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ItemCondi__UserI__75A278F5");
        });

        modelBuilder.Entity<ItemImage>(entity =>
        {
            entity.HasKey(e => e.ItemImageId).HasName("PK__ItemImag__09AE3297C7A40AB4");

            entity.ToTable("ItemImage");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("ImageURL");

            entity.HasOne(d => d.Item).WithMany(p => p.ItemImages)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__ItemImage__ItemI__5629CD9C");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BCF4EEFBAC2");

            entity.ToTable("Order");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Field).HasMaxLength(100);
            entity.Property(e => e.OrderAddress).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Order__UserId__7F2BE32F");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__OrderDet__57ED0681D8407904");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.PriceItem).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__Order__02084FDA");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__OrderDeta__Produ__02FC7413");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A382673280C");

            entity.ToTable("Payment");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TransactionCode).HasMaxLength(255);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Payment__OrderId__07C12930");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentMethodId)
                .HasConstraintName("FK__Payment__Payment__08B54D69");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.PaymentMethodId).HasName("PK__PaymentM__DC31C1D39010B036");

            entity.ToTable("PaymentMethod");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.MethodName).HasMaxLength(100);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6CDB32CECB3");

            entity.ToTable("Product");

            entity.Property(e => e.Cpu)
                .HasMaxLength(50)
                .HasColumnName("CPU");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ImageProduct).HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductName).HasMaxLength(255);
            entity.Property(e => e.Ram)
                .HasMaxLength(50)
                .HasColumnName("RAM");
            entity.Property(e => e.ScreenSize).HasMaxLength(50);
            entity.Property(e => e.Storage).HasMaxLength(50);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Product__Categor__48CFD27E");

            entity.HasOne(d => d.Shop).WithMany(p => p.Products)
                .HasForeignKey(d => d.ShopId)
                .HasConstraintName("FK__Product__ShopId__49C3F6B7");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.ProductImageId).HasName("PK__ProductI__07B2B1B8A9E90C2B");

            entity.ToTable("ProductImage");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("ImageURL");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ProductIm__Produ__4CA06362");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC07D9816131");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpiredAt).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Active");
            entity.Property(e => e.Token).HasMaxLength(500);

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_RefreshToken_User");
        });

        modelBuilder.Entity<RefundTransaction>(entity =>
        {
            entity.HasKey(e => e.RefundId).HasName("PK__RefundTr__725AB920D6AEB5B8");

            entity.ToTable("RefundTransaction");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Order).WithMany(p => p.RefundTransactions)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__RefundTra__Order__17036CC0");

            entity.HasOne(d => d.Payment).WithMany(p => p.RefundTransactions)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK__RefundTra__Payme__17F790F9");

            entity.HasOne(d => d.Wallet).WithMany(p => p.RefundTransactions)
                .HasForeignKey(d => d.WalletId)
                .HasConstraintName("FK__RefundTra__Walle__18EBB532");
        });

        modelBuilder.Entity<ReportDamage>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__ReportDa__D5BD4805C08F6AAC");

            entity.ToTable("ReportDamage");

            entity.Property(e => e.ConditionAfterReturn).HasMaxLength(255);
            entity.Property(e => e.ConditionBeforeBorrow).HasMaxLength(255);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DamageFee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ImageUrlreport)
                .HasMaxLength(255)
                .HasColumnName("ImageURLReport");
            entity.Property(e => e.Note).HasMaxLength(255);

            entity.HasOne(d => d.BorrowHistory).WithMany(p => p.ReportDamages)
                .HasForeignKey(d => d.BorrowHistoryId)
                .HasConstraintName("FK__ReportDam__Borro__6B24EA82");

            entity.HasOne(d => d.Item).WithMany(p => p.ReportDamages)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__ReportDam__ItemI__6A30C649");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1A3FCF8785");

            entity.ToTable("Role");

            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<SettlementTransaction>(entity =>
        {
            entity.HasKey(e => e.SettlementId).HasName("PK__Settleme__7712545A740EB8ED");

            entity.ToTable("SettlementTransaction");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Fee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.ManagerWallet).WithMany(p => p.SettlementTransactionManagerWallets)
                .HasForeignKey(d => d.ManagerWalletId)
                .HasConstraintName("FK__Settlemen__Manag__14270015");

            entity.HasOne(d => d.Order).WithMany(p => p.SettlementTransactions)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Settlemen__Order__123EB7A3");

            entity.HasOne(d => d.ShopWallet).WithMany(p => p.SettlementTransactionShopWallets)
                .HasForeignKey(d => d.ShopWalletId)
                .HasConstraintName("FK__Settlemen__ShopW__1332DBDC");
        });

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasKey(e => e.ShipmentId).HasName("PK__Shipment__5CAD37ED456A5D1B");

            entity.ToTable("Shipment");

            entity.Property(e => e.Carrier).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.EstimatedDeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.ShippingCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TrackingNumber).HasMaxLength(255);

            entity.HasOne(d => d.Order).WithMany(p => p.Shipments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Shipment__OrderI__208CD6FA");
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.ShopId).HasName("PK__Shop__67C557C9DEE21C2E");

            entity.ToTable("Shop");

            entity.HasIndex(e => e.UserId, "UQ__Shop__1788CC4D7AB770C2").IsUnique();

            entity.Property(e => e.BankName).HasMaxLength(100);
            entity.Property(e => e.BankNumber).HasMaxLength(100);
            entity.Property(e => e.BusinessLicense).HasMaxLength(255);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ShopAddress).HasMaxLength(255);
            entity.Property(e => e.ShopName).HasMaxLength(255);
            entity.Property(e => e.ShopPhone).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithOne(p => p.Shop)
                .HasForeignKey<Shop>(d => d.UserId)
                .HasConstraintName("FK__Shop__UserId__403A8C7D");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__32C52B99A5D48135");

            entity.ToTable("Student");

            entity.HasIndex(e => e.UserId, "UQ__Student__1788CC4D198D303E").IsUnique();

            entity.Property(e => e.EnrollmentDate).HasColumnType("datetime");
            entity.Property(e => e.IdentityCard).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.StudentCode).HasMaxLength(50);

            entity.HasOne(d => d.User).WithOne(p => p.Student)
                .HasForeignKey<Student>(d => d.UserId)
                .HasConstraintName("FK__Student__UserId__440B1D61");
        });

        modelBuilder.Entity<TrackingInfo>(entity =>
        {
            entity.HasKey(e => e.TrackingId).HasName("PK__Tracking__3C19EDF1E0EAC690");

            entity.ToTable("TrackingInfo");

            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.TrackingInfos)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__TrackingI__Order__236943A5");

            entity.HasOne(d => d.Shipment).WithMany(p => p.TrackingInfos)
                .HasForeignKey(d => d.ShipmentId)
                .HasConstraintName("FK__TrackingI__Shipm__245D67DE");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4C5E8616AD");

            entity.ToTable("User");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.RoleId).HasDefaultValue(1);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.UsersNavigation)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AccountRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Account_R__RoleI__3C69FB99"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Account_R__UserI__3B75D760"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("PK__Account___AF2760ADDC72158F");
                        j.ToTable("Account_Role");
                    });
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.WalletId).HasName("PK__Wallet__84D4F90E2138CB00");

            entity.ToTable("Wallet");

            entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Wallets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Wallet__UserId__0B91BA14");
        });

        modelBuilder.Entity<WalletTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__WalletTr__55433A6B7E04F240");

            entity.ToTable("WalletTransaction");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.TransactionType).HasMaxLength(50);

            entity.HasOne(d => d.RelatedPayment).WithMany(p => p.WalletTransactions)
                .HasForeignKey(d => d.RelatedPaymentId)
                .HasConstraintName("FK__WalletTra__Relat__0F624AF8");

            entity.HasOne(d => d.Wallet).WithMany(p => p.WalletTransactions)
                .HasForeignKey(d => d.WalletId)
                .HasConstraintName("FK__WalletTra__Walle__0E6E26BF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
