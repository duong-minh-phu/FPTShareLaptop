using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using DataAccess.BorrowHistoryDTO;
using DataAccess.CategoryDTO;
using DataAccess.CompensationTransactionDTO;
using DataAccess.DepositTransactionDTO;
using DataAccess.DonateItemDTO;
using DataAccess.DonationFormDTO;
using DataAccess.FeedbackProductDTO;
using DataAccess.ItemImageDTO;

using DataAccess.PaymentDTO;
using DataAccess.PaymentMethodDTO;
using DataAccess.RefundTransactionDTO;
using DataAccess.ReportDamageDTO;
using DataAccess.WalletDTO;
using DataAccess.WalletTransaction;

using DataAccess.OrderDetailDTO;
using DataAccess.OrderDTO;
using DataAccess.ProductDTO;
using DataAccess.ProductImageDTO;
using DataAccess.ReportDamageDTO;
using DataAccess.SettlementTransactionDTO;
using DataAccess.ShopDTO;
using DataAccess.UserDTO;
using DataAccess.MajorDTO;
using DataAccess.SponsorFundDTO;
using DataAccess.PurchasedLaptopDTO;

using DataAccess.TransactionLogDTO;



namespace Service.Utils.MapperProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DonateItem, DonateItemReadDTO>();
            CreateMap<DonateItemCreateDTO, DonateItem>();
            CreateMap<DonateItemUpdateDTO, DonateItem>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ItemImage, ItemImageDTO>().ReverseMap();
            CreateMap<CreateItemImageDTO, ItemImage>().ReverseMap();
            CreateMap<UpdateItemImageDTO, ItemImage>().ReverseMap();
            CreateMap<BorrowHistory, BorrowHistoryReadDTO>().ReverseMap();
            CreateMap<BorrowHistoryCreateDTO, BorrowHistory>().ReverseMap();
            CreateMap<BorrowHistoryUpdateDTO, BorrowHistory>().ReverseMap();

            CreateMap<ReportDamage, ReportDamageDTO>().ReverseMap();
            CreateMap<ReportDamageCreateDTO, ReportDamage>();
            CreateMap<ReportDamageUpdateDTO, ReportDamage>();


            CreateMap<CompensationTransaction, CompensationTransactionDTO>().ReverseMap();
            CreateMap<CompensationTransactionCreateDTO, CompensationTransaction>();
            CreateMap<CompensationTransactionUpdateDTO, CompensationTransaction>();

            CreateMap<ContractImage, ContractImageDTO>().ReverseMap();
            CreateMap<ContractImageCreateDTO, ContractImage>();

            CreateMap<DepositTransaction, DepositTransactionDTO>();
            CreateMap<DepositTransactionCreateDTO, DepositTransaction>();
            CreateMap<DepositTransactionUpdateDTO, DepositTransaction>();

            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryCreateDTO, Category>();
            CreateMap<CategoryUpdateDTO, Category>();


            CreateMap<Wallet, WalletResModel>().ReverseMap();
            CreateMap<WalletReqModel, Wallet>().ReverseMap();

            CreateMap<WalletTransaction, WalletTransactionResModel>().ReverseMap();
            CreateMap<WalletTransactionReqModel, WalletTransaction>().ReverseMap();

            CreateMap<RefundTransaction, RefundTransactionResModel>().ReverseMap();
            CreateMap<RefundTransactionReqModel, RefundTransaction>().ReverseMap();

            CreateMap<Payment, PaymentViewResModel>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Order.User.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Order.User.FullName));



            CreateMap<PaymentMethod, PaymentMethodResModel>().ReverseMap();
            CreateMap<PaymentMethodReqModel, PaymentMethod>().ReverseMap();




            CreateMap<Shop, ShopReadDTO>();
            CreateMap<ShopCreateDTO, Shop>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<ShopUpdateDTO, Shop>()
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));



            CreateMap<ProductImage, ProductImageReadDTO>();
            CreateMap<ProductImageCreateDTO, ProductImage>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<ProductImageUpdateDTO, ProductImage>();


            CreateMap<Product, ProductReadDTO>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName));
            CreateMap<ProductCreateDTO, Product>();
            CreateMap<ProductUpdateDTO, Product>();

            CreateMap<Order, OrderReadDTO>();
            CreateMap<OrderCreateDTO, Order>();
            CreateMap<OrderUpdateDTO, Order>();


            CreateMap<OrderDetail, OrderDetailReadDTO>();
            CreateMap<OrderDetailCreateDTO, OrderDetail>();
            CreateMap<OrderDetailUpdateDTO, OrderDetail>();


            CreateMap<SettlementTransaction, SettlementTransactionDTO>();
            CreateMap<SettlementTransactionCreateDTO, SettlementTransaction>();
            CreateMap<SettlementTransactionUpdateDTO, SettlementTransaction>();


            CreateMap<Major, MajorReadDTO>();
            CreateMap<MajorCreateDTO, Major>();
            CreateMap<MajorUpdateDTO, Major>();


            CreateMap<SponsorFund, SponsorFundReadDTO>();
            CreateMap<SponsorFundCreateDTO, SponsorFund>();
            CreateMap<SponsorFundUpdateDTO, SponsorFund>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<PurchasedLaptop, PurchasedLaptopDTO>().ReverseMap();
            CreateMap<PurchasedLaptopCreateDTO, PurchasedLaptop>();
            CreateMap<PurchasedLaptopUpdateDTO, PurchasedLaptop>();


            CreateMap<FeedbackProduct, FeedbackProductDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FeedbackProductId))
            .ForMember(dest => dest.OrderDetailId, opt => opt.MapFrom(src => src.OrderItemId))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.FeedbackDate));

            CreateMap<FeedbackProductCreateDTO, FeedbackProduct>()
                .ForMember(dest => dest.OrderItemId, opt => opt.MapFrom(src => src.OrderDetailId))
                .ForMember(dest => dest.FeedbackDate, opt => opt.Ignore());
            CreateMap<FeedbackProductUpdateDTO, FeedbackProduct>();

            // Mapping cho bảng User
            CreateMap<User, UserProfileModel>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.Dob))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))

                // Mapping thông tin từ bảng Student nếu có
                .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src => src.Student != null ? src.Student.StudentCode : null))
                .ForMember(dest => dest.IdentityCard, opt => opt.MapFrom(src => src.Student != null ? src.Student.IdentityCard : null))
                .ForMember(dest => dest.EnrollmentDate, opt => opt.MapFrom(src => src.Student != null ? src.Student.EnrollmentDate : null));


            CreateMap<TransactionLog, TransactionLogResModel>().ReverseMap();

        }
    }
}
