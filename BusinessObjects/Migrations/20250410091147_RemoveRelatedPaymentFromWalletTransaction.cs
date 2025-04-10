using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRelatedPaymentFromWalletTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Category__19093A0B357CC0CE", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MethodName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PaymentM__DC31C1D375FDCC0B", x => x.PaymentMethodId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role__8AFACE1A039A7AD6", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Dob = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__1788CC4C567334F5", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_Role",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "Account_Role",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Account___AF2760AD7FC46D56", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK__Account_R__RoleI__76969D2E",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId");
                    table.ForeignKey(
                        name: "FK__Account_R__UserI__778AC167",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "DonateForm",
                columns: table => new
                {
                    DonateFormId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ItemDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DonateQuantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    ImageDonateForm = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DonateFo__4F9196347E2BA20A", x => x.DonateFormId);
                    table.ForeignKey(
                        name: "FK__DonateFor__UserI__05D8E0BE",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Field = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Order__C3905BCFB5F7EBEE", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK__Order__UserId__14270015",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getutcdate())"),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Active")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RefreshT__3214EC071104B986", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shop",
                columns: table => new
                {
                    ShopId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ShopName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ShopAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ShopPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BusinessLicense = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BankNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Shop__67C557C9C80E05B1", x => x.ShopId);
                    table.ForeignKey(
                        name: "FK__Shop__UserId__25518C17",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StudentCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdentityCard = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EnrollmentDate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Student__32C52B99F0D7E1C8", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK__Student__UserId__2645B050",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Wallet",
                columns: table => new
                {
                    WalletId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Wallet__84D4F90EE21594C4", x => x.WalletId);
                    table.ForeignKey(
                        name: "FK__Wallet__UserId__2A164134",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "DonateItem",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ItemImage = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CPU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RAM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Storage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ScreenSize = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ConditionItem = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TotalBorrowedCount = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DonateFormId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DonateIt__727E838B06C0ECE4", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK__DonateIte__Donat__06CD04F7",
                        column: x => x.DonateFormId,
                        principalTable: "DonateForm",
                        principalColumn: "DonateFormId");
                    table.ForeignKey(
                        name: "FK__DonateIte__UserI__07C12930",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    TransactionCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payment__9B556A3847DE06B6", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK__Payment__OrderId__17036CC0",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId");
                    table.ForeignKey(
                        name: "FK__Payment__Payment__17F790F9",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethod",
                        principalColumn: "PaymentMethodId");
                });

            migrationBuilder.CreateTable(
                name: "Shipment",
                columns: table => new
                {
                    ShipmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    TrackingNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Carrier = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EstimatedDeliveryDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Shipment__5CAD37EDE9E0C32B", x => x.ShipmentId);
                    table.ForeignKey(
                        name: "FK__Shipment__OrderI__245D67DE",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageProduct = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ScreenSize = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Storage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RAM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CPU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ShopId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Product__B40CC6CD8397B5CF", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK__Product__Categor__18EBB532",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId");
                    table.ForeignKey(
                        name: "FK__Product__ShopId__19DFD96B",
                        column: x => x.ShopId,
                        principalTable: "Shop",
                        principalColumn: "ShopId");
                });

            migrationBuilder.CreateTable(
                name: "SettlementTransaction",
                columns: table => new
                {
                    SettlementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ShopWalletId = table.Column<int>(type: "int", nullable: false),
                    ManagerWalletId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Settleme__7712545ABD4522CE", x => x.SettlementId);
                    table.ForeignKey(
                        name: "FK__Settlemen__Manag__2180FB33",
                        column: x => x.ManagerWalletId,
                        principalTable: "Wallet",
                        principalColumn: "WalletId");
                    table.ForeignKey(
                        name: "FK__Settlemen__Order__22751F6C",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId");
                    table.ForeignKey(
                        name: "FK__Settlemen__ShopW__236943A5",
                        column: x => x.ShopWalletId,
                        principalTable: "Wallet",
                        principalColumn: "WalletId");
                });

            migrationBuilder.CreateTable(
                name: "BorrowRequest",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BorrowRe__33A8517A81895B5B", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK__BorrowReq__ItemI__7E37BEF6",
                        column: x => x.ItemId,
                        principalTable: "DonateItem",
                        principalColumn: "ItemId");
                    table.ForeignKey(
                        name: "FK__BorrowReq__UserI__7F2BE32F",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "ItemImage",
                columns: table => new
                {
                    ItemImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ItemImag__09AE3297464CA349", x => x.ItemImageId);
                    table.ForeignKey(
                        name: "FK__ItemImage__ItemI__1332DBDC",
                        column: x => x.ItemId,
                        principalTable: "DonateItem",
                        principalColumn: "ItemId");
                });

            migrationBuilder.CreateTable(
                name: "RefundTransaction",
                columns: table => new
                {
                    RefundId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    WalletId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RefundTr__725AB920C851ED11", x => x.RefundId);
                    table.ForeignKey(
                        name: "FK__RefundTra__Order__1CBC4616",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId");
                    table.ForeignKey(
                        name: "FK__RefundTra__Payme__1DB06A4F",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "PaymentId");
                    table.ForeignKey(
                        name: "FK__RefundTra__Walle__1EA48E88",
                        column: x => x.WalletId,
                        principalTable: "Wallet",
                        principalColumn: "WalletId");
                });

            migrationBuilder.CreateTable(
                name: "WalletTransaction",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletId = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RelatedPaymentId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WalletTr__55433A6B187C0FE1", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK__WalletTra__Relat__2B0A656D",
                        column: x => x.RelatedPaymentId,
                        principalTable: "Payment",
                        principalColumn: "PaymentId");
                    table.ForeignKey(
                        name: "FK__WalletTra__Walle__2BFE89A6",
                        column: x => x.WalletId,
                        principalTable: "Wallet",
                        principalColumn: "WalletId");
                });

            migrationBuilder.CreateTable(
                name: "TrackingInfo",
                columns: table => new
                {
                    TrackingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ShipmentId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tracking__3C19EDF141CD11EE", x => x.TrackingId);
                    table.ForeignKey(
                        name: "FK__TrackingI__Order__2739D489",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId");
                    table.ForeignKey(
                        name: "FK__TrackingI__Shipm__282DF8C2",
                        column: x => x.ShipmentId,
                        principalTable: "Shipment",
                        principalColumn: "ShipmentId");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    PriceItem = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderDet__57ED0681E1F6FC0C", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK__OrderDeta__Order__151B244E",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId");
                    table.ForeignKey(
                        name: "FK__OrderDeta__Produ__160F4887",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "ProductImage",
                columns: table => new
                {
                    ProductImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductI__07B2B1B804B9F7E0", x => x.ProductImageId);
                    table.ForeignKey(
                        name: "FK__ProductIm__Produ__1AD3FDA4",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateTable(
                name: "BorrowContract",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContractDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Terms = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ConditionBorrow = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ItemValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpectedReturnDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BorrowCo__C90D3469203F48B6", x => x.ContractId);
                    table.ForeignKey(
                        name: "FK__BorrowCon__ItemI__787EE5A0",
                        column: x => x.ItemId,
                        principalTable: "DonateItem",
                        principalColumn: "ItemId");
                    table.ForeignKey(
                        name: "FK__BorrowCon__Reque__797309D9",
                        column: x => x.RequestId,
                        principalTable: "BorrowRequest",
                        principalColumn: "RequestId");
                    table.ForeignKey(
                        name: "FK__BorrowCon__UserI__7A672E12",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "BorrowHistory",
                columns: table => new
                {
                    BorrowHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BorrowDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BorrowHi__1F7C51B55B601E04", x => x.BorrowHistoryId);
                    table.ForeignKey(
                        name: "FK__BorrowHis__ItemI__7B5B524B",
                        column: x => x.ItemId,
                        principalTable: "DonateItem",
                        principalColumn: "ItemId");
                    table.ForeignKey(
                        name: "FK__BorrowHis__Reque__7C4F7684",
                        column: x => x.RequestId,
                        principalTable: "BorrowRequest",
                        principalColumn: "RequestId");
                    table.ForeignKey(
                        name: "FK__BorrowHis__UserI__7D439ABD",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "FeedbackProduct",
                columns: table => new
                {
                    FeedbackProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderItemId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FeedbackDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsAnonymous = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__7C6D4B9C3F0EDB68", x => x.FeedbackProductId);
                    table.ForeignKey(
                        name: "FK__FeedbackP__Order__0B91BA14",
                        column: x => x.OrderItemId,
                        principalTable: "OrderDetail",
                        principalColumn: "OrderItemId");
                    table.ForeignKey(
                        name: "FK__FeedbackP__Produ__0C85DE4D",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId");
                    table.ForeignKey(
                        name: "FK__FeedbackP__UserI__0D7A0286",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "DepositTransaction",
                columns: table => new
                {
                    DepositId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DepositDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DepositT__AB60DF710846F698", x => x.DepositId);
                    table.ForeignKey(
                        name: "FK__DepositTr__Contr__03F0984C",
                        column: x => x.ContractId,
                        principalTable: "BorrowContract",
                        principalColumn: "ContractId");
                    table.ForeignKey(
                        name: "FK__DepositTr__UserI__04E4BC85",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "FeedbackBorrow",
                columns: table => new
                {
                    FeedbackBorrowId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BorrowHistoryId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FeedbackDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsAnonymous = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__ACD684ADBBEC2341", x => x.FeedbackBorrowId);
                    table.ForeignKey(
                        name: "FK__FeedbackB__Borro__08B54D69",
                        column: x => x.BorrowHistoryId,
                        principalTable: "BorrowHistory",
                        principalColumn: "BorrowHistoryId");
                    table.ForeignKey(
                        name: "FK__FeedbackB__ItemI__09A971A2",
                        column: x => x.ItemId,
                        principalTable: "DonateItem",
                        principalColumn: "ItemId");
                    table.ForeignKey(
                        name: "FK__FeedbackB__UserI__0A9D95DB",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "ReportDamage",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    BorrowHistoryId = table.Column<int>(type: "int", nullable: false),
                    ImageURLReport = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ConditionBeforeBorrow = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ConditionAfterReturn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DamageFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ReportDa__D5BD4805DC04C9DF", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK__ReportDam__Borro__1F98B2C1",
                        column: x => x.BorrowHistoryId,
                        principalTable: "BorrowHistory",
                        principalColumn: "BorrowHistoryId");
                    table.ForeignKey(
                        name: "FK__ReportDam__ItemI__208CD6FA",
                        column: x => x.ItemId,
                        principalTable: "DonateItem",
                        principalColumn: "ItemId");
                });

            migrationBuilder.CreateTable(
                name: "CompensationTransaction",
                columns: table => new
                {
                    CompensationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ReportDamageId = table.Column<int>(type: "int", nullable: false),
                    DepositTransactionId = table.Column<int>(type: "int", nullable: false),
                    CompensationAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UsedDepositAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExtraPaymentRequired = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Compensa__14AB97591221208A", x => x.CompensationId);
                    table.ForeignKey(
                        name: "FK__Compensat__Contr__00200768",
                        column: x => x.ContractId,
                        principalTable: "BorrowContract",
                        principalColumn: "ContractId");
                    table.ForeignKey(
                        name: "FK__Compensat__Depos__01142BA1",
                        column: x => x.DepositTransactionId,
                        principalTable: "DepositTransaction",
                        principalColumn: "DepositId");
                    table.ForeignKey(
                        name: "FK__Compensat__Repor__02084FDA",
                        column: x => x.ReportDamageId,
                        principalTable: "ReportDamage",
                        principalColumn: "ReportId");
                    table.ForeignKey(
                        name: "FK__Compensat__UserI__02FC7413",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "ItemCondition",
                columns: table => new
                {
                    ConditionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BorrowHistoryId = table.Column<int>(type: "int", nullable: false),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    ConditionType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CheckedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CheckedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    RefundDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ItemCond__37F5C0CFA9C5BD15", x => x.ConditionId);
                    table.ForeignKey(
                        name: "FK__ItemCondi__Borro__0E6E26BF",
                        column: x => x.BorrowHistoryId,
                        principalTable: "BorrowHistory",
                        principalColumn: "BorrowHistoryId");
                    table.ForeignKey(
                        name: "FK__ItemCondi__Contr__0F624AF8",
                        column: x => x.ContractId,
                        principalTable: "BorrowContract",
                        principalColumn: "ContractId");
                    table.ForeignKey(
                        name: "FK__ItemCondi__ItemI__10566F31",
                        column: x => x.ItemId,
                        principalTable: "DonateItem",
                        principalColumn: "ItemId");
                    table.ForeignKey(
                        name: "FK__ItemCondi__Repor__114A936A",
                        column: x => x.ReportId,
                        principalTable: "ReportDamage",
                        principalColumn: "ReportId");
                    table.ForeignKey(
                        name: "FK__ItemCondi__UserI__123EB7A3",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_Role_RoleId",
                table: "Account_Role",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowContract_ItemId",
                table: "BorrowContract",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowContract_RequestId",
                table: "BorrowContract",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowContract_UserId",
                table: "BorrowContract",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowHistory_ItemId",
                table: "BorrowHistory",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowHistory_RequestId",
                table: "BorrowHistory",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowHistory_UserId",
                table: "BorrowHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowRequest_ItemId",
                table: "BorrowRequest",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowRequest_UserId",
                table: "BorrowRequest",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompensationTransaction_ContractId",
                table: "CompensationTransaction",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_CompensationTransaction_DepositTransactionId",
                table: "CompensationTransaction",
                column: "DepositTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompensationTransaction_ReportDamageId",
                table: "CompensationTransaction",
                column: "ReportDamageId");

            migrationBuilder.CreateIndex(
                name: "IX_CompensationTransaction_UserId",
                table: "CompensationTransaction",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositTransaction_ContractId",
                table: "DepositTransaction",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositTransaction_UserId",
                table: "DepositTransaction",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DonateForm_UserId",
                table: "DonateForm",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DonateItem_DonateFormId",
                table: "DonateItem",
                column: "DonateFormId");

            migrationBuilder.CreateIndex(
                name: "IX_DonateItem_UserId",
                table: "DonateItem",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackBorrow_BorrowHistoryId",
                table: "FeedbackBorrow",
                column: "BorrowHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackBorrow_ItemId",
                table: "FeedbackBorrow",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackBorrow_UserId",
                table: "FeedbackBorrow",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackProduct_OrderItemId",
                table: "FeedbackProduct",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackProduct_ProductId",
                table: "FeedbackProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackProduct_UserId",
                table: "FeedbackProduct",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCondition_BorrowHistoryId",
                table: "ItemCondition",
                column: "BorrowHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCondition_ContractId",
                table: "ItemCondition",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCondition_ItemId",
                table: "ItemCondition",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCondition_ReportId",
                table: "ItemCondition",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCondition_UserId",
                table: "ItemCondition",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemImage_ItemId",
                table: "ItemImage",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderId",
                table: "OrderDetail",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ProductId",
                table: "OrderDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_OrderId",
                table: "Payment",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PaymentMethodId",
                table: "Payment",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ShopId",
                table: "Product",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ProductId",
                table: "ProductImage",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundTransaction_OrderId",
                table: "RefundTransaction",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundTransaction_PaymentId",
                table: "RefundTransaction",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundTransaction_WalletId",
                table: "RefundTransaction",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportDamage_BorrowHistoryId",
                table: "ReportDamage",
                column: "BorrowHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportDamage_ItemId",
                table: "ReportDamage",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SettlementTransaction_ManagerWalletId",
                table: "SettlementTransaction",
                column: "ManagerWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_SettlementTransaction_OrderId",
                table: "SettlementTransaction",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_SettlementTransaction_ShopWalletId",
                table: "SettlementTransaction",
                column: "ShopWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_OrderId",
                table: "Shipment",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "UQ__Shop__1788CC4D22073DBF",
                table: "Shop",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Student__1788CC4DE28A1826",
                table: "Student",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackingInfo_OrderId",
                table: "TrackingInfo",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingInfo_ShipmentId",
                table: "TrackingInfo",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_UserId",
                table: "Wallet",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransaction_RelatedPaymentId",
                table: "WalletTransaction",
                column: "RelatedPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransaction_WalletId",
                table: "WalletTransaction",
                column: "WalletId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account_Role");

            migrationBuilder.DropTable(
                name: "CompensationTransaction");

            migrationBuilder.DropTable(
                name: "FeedbackBorrow");

            migrationBuilder.DropTable(
                name: "FeedbackProduct");

            migrationBuilder.DropTable(
                name: "ItemCondition");

            migrationBuilder.DropTable(
                name: "ItemImage");

            migrationBuilder.DropTable(
                name: "ProductImage");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "RefundTransaction");

            migrationBuilder.DropTable(
                name: "SettlementTransaction");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "TrackingInfo");

            migrationBuilder.DropTable(
                name: "WalletTransaction");

            migrationBuilder.DropTable(
                name: "DepositTransaction");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "ReportDamage");

            migrationBuilder.DropTable(
                name: "Shipment");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Wallet");

            migrationBuilder.DropTable(
                name: "BorrowContract");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "BorrowHistory");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Shop");

            migrationBuilder.DropTable(
                name: "BorrowRequest");

            migrationBuilder.DropTable(
                name: "DonateItem");

            migrationBuilder.DropTable(
                name: "DonateForm");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
