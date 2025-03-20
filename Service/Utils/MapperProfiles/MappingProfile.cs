using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using DataAccess.BorrowHistoryDTO;
using DataAccess.CompensationTransactionDTO;
using DataAccess.DepositTransactionDTO;
using DataAccess.DonateItemDTO;
using DataAccess.DonationFormDTO;
using DataAccess.ItemImageDTO;
using DataAccess.ReportDamageDTO;

namespace Service.Utils.MapperProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DonateItem, DonateItemDTO>().ReverseMap();
            CreateMap<ItemImage, ItemImageDTO>().ReverseMap();
            CreateMap<CreateItemImageDTO, ItemImage>().ReverseMap();
            CreateMap<UpdateItemImageDTO, ItemImage>().ReverseMap();
            CreateMap<BorrowHistory, BorrowHistoryDTO>().ReverseMap();
            CreateMap<ReportDamage, ReportDamageDTO>().ReverseMap();
            CreateMap<CompensationTransaction, CompensationTransactionDTO>().ReverseMap();
            CreateMap<DepositTransaction, DepositTransactionDTO>().ReverseMap();
        }
    }
}
