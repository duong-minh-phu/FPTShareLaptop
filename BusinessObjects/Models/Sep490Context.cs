using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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

    public virtual DbSet<ContractImage> ContractImages { get; set; }

    public virtual DbSet<DepositTransaction> DepositTransactions { get; set; }

    public virtual DbSet<DonateForm> DonateForms { get; set; }

    public virtual DbSet<DonateItem> DonateItems { get; set; }

    public virtual DbSet<FeedbackBorrow> FeedbackBorrows { get; set; }

    public virtual DbSet<FeedbackProduct> FeedbackProducts { get; set; }

    public virtual DbSet<ItemCondition> ItemConditions { get; set; }

    public virtual DbSet<ItemImage> ItemImages { get; set; }

    public virtual DbSet<Major> Majors { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<PurchasedLaptop> PurchasedLaptops { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<RefundTransaction> RefundTransactions { get; set; }

    public virtual DbSet<ReportDamage> ReportDamages { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SettlementTransaction> SettlementTransactions { get; set; }

    public virtual DbSet<Shipment> Shipments { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<SponsorContribution> SponsorContributions { get; set; }

    public virtual DbSet<SponsorFund> SponsorFunds { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<TrackingInfo> TrackingInfos { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    public virtual DbSet<WalletTransaction> WalletTransactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);Uid=sa;Pwd=12345;Database=SEP490;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BorrowContract>(entity =>
        {
            entity.HasKey(e => e.ContractId).HasName("PK__BorrowCo__C90D34695F00D2DA");

            entity.ToTable("BorrowContract");

            entity.Property(e => e.ConditionBorrow)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.ContractDate).HasColumnType("datetime");
            entity.Property(e => e.ExpectedReturnDate).HasColumnType("datetime");
            entity.Property(e => e.ItemValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Terms)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Item).WithMany(p => p.BorrowContracts)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BorrowCon__ItemI__02084FDA");

            entity.HasOne(d => d.Request).WithMany(p => p.BorrowContracts)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BorrowCon__Reque__02FC7413");

            entity.HasOne(d => d.User).WithMany(p => p.BorrowContracts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BorrowCon__UserI__03F0984C");
        });

        modelBuilder.Entity<BorrowHistory>(entity =>
        {
            entity.HasKey(e => e.BorrowHistoryId).HasName("PK__BorrowHi__1F7C51B5235F8FFD");

            entity.ToTable("BorrowHistory");

            entity.Property(e => e.BorrowDate).HasColumnType("datetime");
            entity.Property(e => e.ReturnDate).HasColumnType("datetime");

            entity.HasOne(d => d.Item).WithMany(p => p.BorrowHistories)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BorrowHis__ItemI__04E4BC85");

            entity.HasOne(d => d.Request).WithMany(p => p.BorrowHistories)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BorrowHis__Reque__05D8E0BE");

            entity.HasOne(d => d.User).WithMany(p => p.BorrowHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BorrowHis__UserI__06CD04F7");
        });

        modelBuilder.Entity<BorrowRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__BorrowRe__33A8517A3D5B0C4C");

            entity.ToTable("BorrowRequest");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Item).WithMany(p => p.BorrowRequests)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BorrowReq__ItemI__07C12930");

            entity.HasOne(d => d.Major).WithMany(p => p.BorrowRequests)
                .HasForeignKey(d => d.MajorId)
                .HasConstraintName("FK_BorrowRequest_Major");

            entity.HasOne(d => d.User).WithMany(p => p.BorrowRequests)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BorrowReq__UserI__08B54D69");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B0B40925D");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryName)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<CompensationTransaction>(entity =>
        {
            entity.HasKey(e => e.CompensationId).HasName("PK__Compensa__14AB9759359C891A");

            entity.ToTable("CompensationTransaction");

            entity.Property(e => e.CompensationAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ExtraPaymentRequired).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.UsedDepositAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Contract).WithMany(p => p.CompensationTransactions)
                .HasForeignKey(d => d.ContractId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Compensat__Contr__09A971A2");

            entity.HasOne(d => d.DepositTransaction).WithMany(p => p.CompensationTransactions)
                .HasForeignKey(d => d.DepositTransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Compensat__Depos__0A9D95DB");

            entity.HasOne(d => d.ReportDamage).WithMany(p => p.CompensationTransactions)
                .HasForeignKey(d => d.ReportDamageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Compensat__Repor__0B91BA14");

            entity.HasOne(d => d.User).WithMany(p => p.CompensationTransactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Compensat__UserI__0C85DE4D");
        });

        modelBuilder.Entity<ContractImage>(entity =>
        {
            entity.HasKey(e => e.ContractImageId).HasName("PK__Contract__0952C38CEFEFD931");

            entity.ToTable("ContractImage");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            entity.HasOne(d => d.BorrowContract).WithMany(p => p.ContractImages)
                .HasForeignKey(d => d.BorrowContractId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ContractImage_BorrowContract");
        });

        modelBuilder.Entity<DepositTransaction>(entity =>
        {
            entity.HasKey(e => e.DepositId).HasName("PK__DepositT__AB60DF7166A346A1");

            entity.ToTable("DepositTransaction");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DepositDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.Contract).WithMany(p => p.DepositTransactions)
                .HasForeignKey(d => d.ContractId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DepositTr__Contr__0D7A0286");

            entity.HasOne(d => d.User).WithMany(p => p.DepositTransactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DepositTr__UserI__0E6E26BF");
        });

        modelBuilder.Entity<DonateForm>(entity =>
        {
            entity.HasKey(e => e.DonateFormId).HasName("PK__DonateFo__4F9196341CE25E44");

            entity.ToTable("DonateForm");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DonateQuantity).HasDefaultValue(1);
            entity.Property(e => e.ImageDonateForm).HasMaxLength(250);
            entity.Property(e => e.ItemDescription)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.ItemName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.DonateForms)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonateFor__UserI__0F624AF8");
        });

        modelBuilder.Entity<DonateItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__DonateIt__727E838B9919E24C");

            entity.ToTable("DonateItem");

            entity.Property(e => e.Battery)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.CategoryId).HasDefaultValue(1);
            entity.Property(e => e.Color)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("");
            entity.Property(e => e.ConditionItem)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Cpu)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("CPU");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasDefaultValue("");
            entity.Property(e => e.GraphicsCard)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.ItemImage)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.ItemName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Model)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.OperatingSystem)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.Ports)
                .IsRequired()
                .HasMaxLength(255)
                .HasDefaultValue("");
            entity.Property(e => e.ProductionYear).HasDefaultValue(2000);
            entity.Property(e => e.Ram)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("RAM");
            entity.Property(e => e.ScreenSize)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.SerialNumber)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Storage)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.DonateItems)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DonateItem_Category");

            entity.HasOne(d => d.DonateForm).WithMany(p => p.DonateItems)
                .HasForeignKey(d => d.DonateFormId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DonateIte__Donat__10566F31");

            entity.HasOne(d => d.User).WithMany(p => p.DonateItems)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__DonateIte__UserI__114A936A");
        });

        modelBuilder.Entity<FeedbackBorrow>(entity =>
        {
            entity.HasKey(e => e.FeedbackBorrowId).HasName("PK__Feedback__ACD684ADB841B0DB");

            entity.ToTable("FeedbackBorrow");

            entity.Property(e => e.Comments)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.FeedbackDate).HasColumnType("datetime");

            entity.HasOne(d => d.BorrowHistory).WithMany(p => p.FeedbackBorrows)
                .HasForeignKey(d => d.BorrowHistoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FeedbackB__Borro__1332DBDC");

            entity.HasOne(d => d.Item).WithMany(p => p.FeedbackBorrows)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FeedbackB__ItemI__14270015");

            entity.HasOne(d => d.User).WithMany(p => p.FeedbackBorrows)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FeedbackB__UserI__151B244E");
        });

        modelBuilder.Entity<FeedbackProduct>(entity =>
        {
            entity.HasKey(e => e.FeedbackProductId).HasName("PK__Feedback__7C6D4B9C4B7F15EE");

            entity.ToTable("FeedbackProduct");

            entity.Property(e => e.Comments)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.FeedbackDate).HasColumnType("datetime");

            entity.HasOne(d => d.OrderItem).WithMany(p => p.FeedbackProducts)
                .HasForeignKey(d => d.OrderItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FeedbackP__Order__160F4887");

            entity.HasOne(d => d.Product).WithMany(p => p.FeedbackProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FeedbackP__Produ__17036CC0");

            entity.HasOne(d => d.User).WithMany(p => p.FeedbackProducts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FeedbackP__UserI__17F790F9");
        });

        modelBuilder.Entity<ItemCondition>(entity =>
        {
            entity.HasKey(e => e.ConditionId).HasName("PK__ItemCond__37F5C0CF85A03D6F");

            entity.ToTable("ItemCondition");

            entity.Property(e => e.CheckedBy)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.CheckedDate).HasColumnType("datetime");
            entity.Property(e => e.ConditionType)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.ImageUrl)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("ImageURL");
            entity.Property(e => e.RefundDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20);

            entity.HasOne(d => d.BorrowHistory).WithMany(p => p.ItemConditions)
                .HasForeignKey(d => d.BorrowHistoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemCondi__Borro__18EBB532");

            entity.HasOne(d => d.Contract).WithMany(p => p.ItemConditions)
                .HasForeignKey(d => d.ContractId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemCondi__Contr__19DFD96B");

            entity.HasOne(d => d.Item).WithMany(p => p.ItemConditions)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemCondi__ItemI__1AD3FDA4");

            entity.HasOne(d => d.Report).WithMany(p => p.ItemConditions)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemCondi__Repor__1BC821DD");

            entity.HasOne(d => d.User).WithMany(p => p.ItemConditions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemCondi__UserI__1CBC4616");
        });

        modelBuilder.Entity<ItemImage>(entity =>
        {
            entity.HasKey(e => e.ItemImageId).HasName("PK__ItemImag__09AE32970798E9F6");

            entity.ToTable("ItemImage");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("ImageURL");

            entity.HasOne(d => d.Item).WithMany(p => p.ItemImages)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ItemImage__ItemI__1DB06A4F");
        });

        modelBuilder.Entity<Major>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Major__3214EC07A406ACEC");

            entity.ToTable("Major");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BCF60B1E9C2");

            entity.ToTable("Order");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Field)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.OrderAddress)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__UserId__1EA48E88");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__OrderDet__57ED06812DE928D0");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.PriceItem).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__1F98B2C1");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Produ__208CD6FA");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A38E5818796");

            entity.ToTable("Payment");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.TransactionCode)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__OrderId__2180FB33");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payment__Payment__22751F6C");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.PaymentMethodId).HasName("PK__PaymentM__DC31C1D3F5DD1D1D");

            entity.ToTable("PaymentMethod");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.MethodName)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6CD2F9D8C93");

            entity.ToTable("Product");

            entity.Property(e => e.Battery)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Color)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Cpu)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("CPU");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.GraphicsCard)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.ImageProduct)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Model)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.OperatingSystem)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Ports)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Ram)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("RAM");
            entity.Property(e => e.ScreenSize)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Storage)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__Categor__236943A5");

            entity.HasOne(d => d.Shop).WithMany(p => p.Products)
                .HasForeignKey(d => d.ShopId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__ShopId__245D67DE");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.ProductImageId).HasName("PK__ProductI__07B2B1B8D07304B5");

            entity.ToTable("ProductImage");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("ImageURL");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductIm__Produ__25518C17");
        });

        modelBuilder.Entity<PurchasedLaptop>(entity =>
        {
            entity.HasKey(e => e.PurchasedLaptopId).HasName("PK__Purchase__B254BA9F70EB2B17");

            entity.ToTable("PurchasedLaptop");

            entity.Property(e => e.InvoiceImageUrl).HasMaxLength(500);
            entity.Property(e => e.PurchasedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Item).WithMany(p => p.PurchasedLaptops)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Purchased__ItemI__531856C7");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC07C75E26AE");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpiredAt).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Active");
            entity.Property(e => e.Token)
                .IsRequired()
                .HasMaxLength(500);

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_RefreshToken_User");
        });

        modelBuilder.Entity<RefundTransaction>(entity =>
        {
            entity.HasKey(e => e.RefundId).HasName("PK__RefundTr__725AB920F68335D4");

            entity.ToTable("RefundTransaction");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.Order).WithMany(p => p.RefundTransactions)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefundTra__Order__2739D489");

            entity.HasOne(d => d.Payment).WithMany(p => p.RefundTransactions)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefundTra__Payme__282DF8C2");

            entity.HasOne(d => d.Wallet).WithMany(p => p.RefundTransactions)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefundTra__Walle__29221CFB");
        });

        modelBuilder.Entity<ReportDamage>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__ReportDa__D5BD4805C8A9EE29");

            entity.ToTable("ReportDamage");

            entity.Property(e => e.ConditionAfterReturn)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.ConditionBeforeBorrow)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DamageFee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ImageUrlreport)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("ImageURLReport");
            entity.Property(e => e.Note)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.BorrowHistory).WithMany(p => p.ReportDamages)
                .HasForeignKey(d => d.BorrowHistoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReportDam__Borro__2A164134");

            entity.HasOne(d => d.Item).WithMany(p => p.ReportDamages)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ReportDam__ItemI__2B0A656D");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE1AE6B456AC");

            entity.ToTable("Role");

            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<SettlementTransaction>(entity =>
        {
            entity.HasKey(e => e.SettlementId).HasName("PK__Settleme__7712545AFB669DEA");

            entity.ToTable("SettlementTransaction");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Fee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.ManagerWallet).WithMany(p => p.SettlementTransactionManagerWallets)
                .HasForeignKey(d => d.ManagerWalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Settlemen__Manag__2BFE89A6");

            entity.HasOne(d => d.Order).WithMany(p => p.SettlementTransactions)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Settlemen__Order__2CF2ADDF");

            entity.HasOne(d => d.ShopWallet).WithMany(p => p.SettlementTransactionShopWallets)
                .HasForeignKey(d => d.ShopWalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Settlemen__ShopW__2DE6D218");
        });

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasKey(e => e.ShipmentId).HasName("PK__Shipment__5CAD37EDCE16C641");

            entity.ToTable("Shipment");

            entity.Property(e => e.Carrier)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.EstimatedDeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.ShippingCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.TrackingNumber)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Order).WithMany(p => p.Shipments)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Shipment__OrderI__2EDAF651");
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.ShopId).HasName("PK__Shop__67C557C9352E9B7F");

            entity.ToTable("Shop");

            entity.HasIndex(e => e.UserId, "UQ__Shop__1788CC4D11C835A3").IsUnique();

            entity.Property(e => e.BankName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.BankNumber)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.BusinessLicense)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ShopAddress)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.ShopName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.ShopPhone)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithOne(p => p.Shop)
                .HasForeignKey<Shop>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Shop__UserId__2FCF1A8A");
        });

        modelBuilder.Entity<SponsorContribution>(entity =>
        {
            entity.HasKey(e => e.SponsorContributionId).HasName("PK__SponsorC__E26AF2C6FAD34366");

            entity.ToTable("SponsorContribution");

            entity.Property(e => e.ContributedAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.PurchasedLaptop).WithMany(p => p.SponsorContributions)
                .HasForeignKey(d => d.PurchasedLaptopId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SponsorCo__Purch__56E8E7AB");

            entity.HasOne(d => d.SponsorFund).WithMany(p => p.SponsorContributions)
                .HasForeignKey(d => d.SponsorFundId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SponsorCo__Spons__55F4C372");
        });

        modelBuilder.Entity<SponsorFund>(entity =>
        {
            entity.HasKey(e => e.SponsorFundId).HasName("PK__SponsorF__F969C5C51AC9181B");

            entity.ToTable("SponsorFund");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.ProofImageUrl).HasMaxLength(500);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.TransferDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.SponsorFunds)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SponsorFu__UserI__4F47C5E3");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Student__32C52B99EE89A6B5");

            entity.ToTable("Student");

            entity.HasIndex(e => e.UserId, "UQ__Student__1788CC4D8342CB2A").IsUnique();

            entity.Property(e => e.EnrollmentDate)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.IdentityCard)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.StudentCode)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.User).WithOne(p => p.Student)
                .HasForeignKey<Student>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Student__UserId__30C33EC3");
        });

        modelBuilder.Entity<TrackingInfo>(entity =>
        {
            entity.HasKey(e => e.TrackingId).HasName("PK__Tracking__3C19EDF1ABEB9AE9");

            entity.ToTable("TrackingInfo");

            entity.Property(e => e.Location)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.TrackingInfos)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TrackingI__Order__31B762FC");

            entity.HasOne(d => d.Shipment).WithMany(p => p.TrackingInfos)
                .HasForeignKey(d => d.ShipmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TrackingI__Shipm__32AB8735");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4C4DF31618");

            entity.ToTable("User");

            entity.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Avatar)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Dob)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Gender)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.RoleId).HasDefaultValue(1);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);

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
                        .HasConstraintName("FK__Account_R__RoleI__00200768"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Account_R__UserI__01142BA1"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("PK__Account___AF2760AD67904098");
                        j.ToTable("Account_Role");
                    });
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.WalletId).HasName("PK__Wallet__84D4F90E5712A7BB");

            entity.ToTable("Wallet");

            entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Wallets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Wallet__UserId__3493CFA7");
        });

        modelBuilder.Entity<WalletTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__WalletTr__55433A6B1843C26A");

            entity.ToTable("WalletTransaction");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Note)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.TransactionType)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.Wallet).WithMany(p => p.WalletTransactions)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__WalletTra__Walle__3587F3E0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
