﻿using System;
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
using DataAccess.OrderDetailDTO;
using DataAccess.OrderDTO;
using DataAccess.ProductDTO;
using DataAccess.ProductImageDTO;
using DataAccess.ReportDamageDTO;
using DataAccess.ShopDTO;

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
        }
    }
}
