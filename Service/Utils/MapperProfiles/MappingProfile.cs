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
using DataAccess.ItemImageDTO;
using DataAccess.PaymentDTO;
using DataAccess.PaymentMethodDTO;
using DataAccess.RefundTransactionDTO;
using DataAccess.ReportDamageDTO;
using DataAccess.WalletDTO;
using DataAccess.WalletTransaction;

namespace Service.Utils.MapperProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DonateItem, DonateItemDTO>();
            CreateMap<DonateItemCreateDTO, DonateItem>();
            CreateMap<DonateItemUpdateDTO, DonateItem>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
            CreateMap<ItemImage, ItemImageDTO>().ReverseMap();
            CreateMap<CreateItemImageDTO, ItemImage>().ReverseMap();
            CreateMap<UpdateItemImageDTO, ItemImage>().ReverseMap();
            CreateMap<BorrowHistory, BorrowHistoryDTO>().ReverseMap();
            CreateMap<ReportDamage, ReportDamageDTO>().ReverseMap();
            CreateMap<ReportDamageCreateDTO, ReportDamage>();
            CreateMap<ReportDamageUpdateDTO, ReportDamage>();
            CreateMap<CompensationTransaction, CompensationTransactionDTO>().ReverseMap();
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

            CreateMap<Payment, PaymentResModel>();          
            CreateMap<PaymentReqModel, Payment>()
                .ForMember(dest => dest.PaymentId, opt => opt.Ignore()) 
                .ForMember(dest => dest.PaymentDate, opt => opt.Ignore()) 
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore()) 
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());

            CreateMap<PaymentMethod, PaymentMethodResModel>().ReverseMap();
            CreateMap<PaymentMethodReqModel, PaymentMethod>();

        }
    }
}
