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

    public virtual DbSet<BorrowDetail> BorrowDetails { get; set; }

    public virtual DbSet<BorrowHistory> BorrowHistories { get; set; }

    public virtual DbSet<BorrowRequest> BorrowRequests { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryItem> CategoryItems { get; set; }

    public virtual DbSet<Commitment> Commitments { get; set; }

    public virtual DbSet<DonationForm> DonationForms { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<LaptopCondition> LaptopConditions { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Shipment> Shipments { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<Sponsor> Sponsors { get; set; }

    public virtual DbSet<SponsorItem> SponsorItems { get; set; }

    public virtual DbSet<SponsorItemImage> SponsorItemImages { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=KHANG\\SQLEXPRESS;Uid=sa;Pwd=12345;Database=SEP490;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BorrowDetail>(entity =>
        {
            entity.HasKey(e => e.BorrowDetailId).HasName("PK__BorrowDe__2D670166C7ED2C05");

            entity.Property(e => e.BorrowDetailId).HasColumnName("BorrowDetailID");
            entity.Property(e => e.ConditionOnReturn).HasMaxLength(255);
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.PickupId).HasColumnName("PickupID");
            entity.Property(e => e.RequestId).HasColumnName("RequestID");
            entity.Property(e => e.ReturnedDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Request).WithMany(p => p.BorrowDetails)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("FK__BorrowDet__Reque__59063A47");
        });

        modelBuilder.Entity<BorrowHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__BorrowHi__4D7B4ADDBB8B10A3");

            entity.ToTable("BorrowHistory");

            entity.Property(e => e.HistoryId).HasColumnName("HistoryID");
            entity.Property(e => e.BorrowDate).HasColumnType("datetime");
            entity.Property(e => e.BorrowDetailId).HasColumnName("BorrowDetailID");
            entity.Property(e => e.LaptopId).HasColumnName("LaptopID");
            entity.Property(e => e.ReturnDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Student).WithMany(p => p.BorrowHistories)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__BorrowHis__Stude__5BE2A6F2");
        });

        modelBuilder.Entity<BorrowRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__BorrowRe__33A8519A5258F0F2");

            entity.Property(e => e.RequestId).HasColumnName("RequestID");
            entity.Property(e => e.ApprovalDate).HasColumnType("datetime");
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RequestStatus).HasMaxLength(50);
            entity.Property(e => e.SponsorLaptopId).HasColumnName("SponsorLaptopID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Student).WithMany(p => p.BorrowRequests)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__BorrowReq__Stude__5629CD9C");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2B7649EA71");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(255);
        });

        modelBuilder.Entity<CategoryItem>(entity =>
        {
            entity.HasKey(e => e.CategoryItemId).HasName("PK__Category__E04E11001262DA70");

            entity.Property(e => e.CategoryItemId).HasColumnName("CategoryItemID");
            entity.Property(e => e.CategoryItemName).HasMaxLength(255);
        });

        modelBuilder.Entity<Commitment>(entity =>
        {
            entity.HasKey(e => e.CommitmentId).HasName("PK__Commitme__5360E89755D2767C");

            entity.Property(e => e.CommitmentId).HasColumnName("CommitmentID");
            entity.Property(e => e.SignedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SponsorId).HasColumnName("SponsorID");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Sponsor).WithMany(p => p.Commitments)
                .HasForeignKey(d => d.SponsorId)
                .HasConstraintName("FK__Commitmen__Spons__7E37BEF6");

            entity.HasOne(d => d.Student).WithMany(p => p.Commitments)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Commitmen__Stude__7D439ABD");
        });

        modelBuilder.Entity<DonationForm>(entity =>
        {
            entity.HasKey(e => e.DonationFormId).HasName("PK__Donation__263B91D34A5C58E5");

            entity.Property(e => e.DonationFormId).HasColumnName("DonationFormID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ItemDescription).HasMaxLength(255);
            entity.Property(e => e.ItemName).HasMaxLength(255);
            entity.Property(e => e.SponsorId).HasColumnName("SponsorID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Sponsor).WithMany(p => p.DonationForms)
                .HasForeignKey(d => d.SponsorId)
                .HasConstraintName("FK__DonationF__Spons__440B1D61");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__6A4BEDF6930B23B9");

            entity.Property(e => e.FeedbackId).HasColumnName("FeedbackID");
            entity.Property(e => e.Comments).HasMaxLength(255);
            entity.Property(e => e.FeedbackDate).HasColumnType("datetime");
            entity.Property(e => e.HistoryId).HasColumnName("HistoryID");
            entity.Property(e => e.LaptopId).HasColumnName("LaptopID");
            entity.Property(e => e.StudentId).HasColumnName("StudentID");

            entity.HasOne(d => d.Student).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__Feedbacks__Stude__5EBF139D");
        });

        modelBuilder.Entity<LaptopCondition>(entity =>
        {
            entity.HasKey(e => e.ConditionId).HasName("PK__LaptopCo__37F5C0EF49338230");

            entity.Property(e => e.ConditionId).HasColumnName("ConditionID");
            entity.Property(e => e.BorrowDetailId).HasColumnName("BorrowDetailID");
            entity.Property(e => e.ConditionAfter).HasMaxLength(255);
            entity.Property(e => e.ConditionBefore).HasMaxLength(255);
            entity.Property(e => e.RepairCost).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.BorrowDetail).WithMany(p => p.LaptopConditions)
                .HasForeignKey(d => d.BorrowDetailId)
                .HasConstraintName("FK__LaptopCon__Borro__787EE5A0");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAF33A61BDA");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Orders__UserId__6B24EA82");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__OrderIte__57ED06A1457F5FEA");

            entity.Property(e => e.OrderItemId).HasColumnName("OrderItemID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderItem__Order__6E01572D");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__OrderItem__Produ__6EF57B66");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6EDC37A3715");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.BatteryLife).HasMaxLength(255);
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Cpu)
                .HasMaxLength(255)
                .HasColumnName("CPU");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Gpu)
                .HasMaxLength(255)
                .HasColumnName("GPU");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductName).HasMaxLength(255);
            entity.Property(e => e.Ram)
                .HasMaxLength(50)
                .HasColumnName("RAM");
            entity.Property(e => e.ScreenSize).HasMaxLength(50);
            entity.Property(e => e.ShopId).HasColumnName("ShopID");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Storage).HasMaxLength(255);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Products__Catego__6754599E");

            entity.HasOne(d => d.Shop).WithMany(p => p.Products)
                .HasForeignKey(d => d.ShopId)
                .HasConstraintName("FK__Products__ShopID__66603565");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__ProductI__7516F4EC17491D44");

            entity.Property(e => e.ImageId).HasColumnName("ImageID");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("ImageURL");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ProductIm__Produ__71D1E811");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.RefreshTokenId).HasName("PK__RefreshT__F5845E5965DEED16");

            entity.Property(e => e.RefreshTokenId).HasColumnName("RefreshTokenID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpiredAt).HasColumnType("datetime");
            entity.Property(e => e.RevokedAt).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Token).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RefreshTo__UserI__02FC7413");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3AFAFD9F36");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160C8061085").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasKey(e => e.ShipmentId).HasName("PK__Shipment__5CAD378D0E043BFB");

            entity.Property(e => e.ShipmentId).HasColumnName("ShipmentID");
            entity.Property(e => e.ActualDeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.EstimatedDeliveryDate).HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ShipperName).HasMaxLength(255);
            entity.Property(e => e.ShippingAddress).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TrackingNumber).HasMaxLength(50);

            entity.HasOne(d => d.Order).WithMany(p => p.Shipments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Shipments__Order__75A278F5");
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.ShopId).HasName("PK__Shops__67C556295828AD3A");

            entity.HasIndex(e => e.UserId, "UQ__Shops__1788CCAD617F6D01").IsUnique();

            entity.Property(e => e.ShopId).HasColumnName("ShopID");
            entity.Property(e => e.ContactInfo).HasMaxLength(255);
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ShopLocation).HasMaxLength(255);
            entity.Property(e => e.ShopName).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TaxCode).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithOne(p => p.Shop)
                .HasForeignKey<Shop>(d => d.UserId)
                .HasConstraintName("FK__Shops__UserID__6383C8BA");
        });

        modelBuilder.Entity<Sponsor>(entity =>
        {
            entity.HasKey(e => e.SponsorId).HasName("PK__Sponsors__3B609EF597691825");

            entity.HasIndex(e => e.UserId, "UQ__Sponsors__1788CCAD8CB196AF").IsUnique();

            entity.Property(e => e.SponsorId).HasColumnName("SponsorID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ContactEmail).HasMaxLength(255);
            entity.Property(e => e.ContactPhone).HasMaxLength(20);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithOne(p => p.Sponsor)
                .HasForeignKey<Sponsor>(d => d.UserId)
                .HasConstraintName("FK__Sponsors__UserID__403A8C7D");
        });

        modelBuilder.Entity<SponsorItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__SponsorI__727E83EB2B92D3B1");

            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.BatteryLife).HasMaxLength(255);
            entity.Property(e => e.CategoryItemId).HasColumnName("CategoryItemID");
            entity.Property(e => e.Condition).HasMaxLength(255);
            entity.Property(e => e.Cpu)
                .HasMaxLength(255)
                .HasColumnName("CPU");
            entity.Property(e => e.CurrentStatus).HasMaxLength(50);
            entity.Property(e => e.DonationFormId).HasColumnName("DonationFormID");
            entity.Property(e => e.Gpu)
                .HasMaxLength(255)
                .HasColumnName("GPU");
            entity.Property(e => e.ItemName).HasMaxLength(255);
            entity.Property(e => e.LastCheckedAt).HasColumnType("datetime");
            entity.Property(e => e.Ram)
                .HasMaxLength(50)
                .HasColumnName("RAM");
            entity.Property(e => e.ScreenSize).HasMaxLength(50);
            entity.Property(e => e.Specifications).HasMaxLength(255);
            entity.Property(e => e.Storage).HasMaxLength(255);

            entity.HasOne(d => d.CategoryItem).WithMany(p => p.SponsorItems)
                .HasForeignKey(d => d.CategoryItemId)
                .HasConstraintName("FK__SponsorIt__Categ__4BAC3F29");

            entity.HasOne(d => d.DonationForm).WithMany(p => p.SponsorItems)
                .HasForeignKey(d => d.DonationFormId)
                .HasConstraintName("FK__SponsorIt__Donat__4AB81AF0");
        });

        modelBuilder.Entity<SponsorItemImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__SponsorI__7516F4ECA4F8F817");

            entity.Property(e => e.ImageId).HasColumnName("ImageID");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("ImageURL");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");

            entity.HasOne(d => d.Item).WithMany(p => p.SponsorItemImages)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__SponsorIt__ItemI__4E88ABD4");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52A79F8AF2D6D");

            entity.HasIndex(e => e.UserId, "UQ__Students__1788CCAD98B3FEE8").IsUnique();

            entity.Property(e => e.StudentId).HasColumnName("StudentID");
            entity.Property(e => e.StudentCode).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithOne(p => p.Student)
                .HasForeignKey<Student>(d => d.UserId)
                .HasConstraintName("FK__Students__UserID__52593CB8");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C10621162");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053477893375").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.ProfilePicture).HasMaxLength(255);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Users__RoleID__3C69FB99");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
