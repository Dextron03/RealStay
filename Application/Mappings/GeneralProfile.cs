using Application.DTOs.Administrator.Agent;
using Application.DTOs.Administrator.Improvements;
using Application.DTOs.Administrator.Properties;
using Application.DTOs.Administrator.Sales;
using Application.DTOs.Administrator.User;
using Application.DTOs.Agent;
using Application.ViewModels.Administrator.Agent;
using Application.ViewModels.Administrator.Improvements;
using Application.ViewModels.Administrator.Properties;
using Application.ViewModels.Administrator.Sales;
using Application.ViewModels.Administrator.User;
using Application.ViewModels.Agent.Profile;
using Application.ViewModels.Agent.Properties;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            //Property Type
            CreateMap<PropertyType, PropertiesDto>()
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PropertyCount, opt => opt.MapFrom(src => src.Properties.Count));
            CreateMap<CreatePropertyDto, PropertyType>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TypeName));
            CreateMap<EditPropertyDto, PropertyType>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TypeName));
            CreateMap<PropertyType, PropertiesViewModel>()
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PropertyCount, opt => opt.MapFrom(src => src.Properties.Count));

            // TypeSale
            CreateMap<TypeSale, SalesTypeDto>()
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PropertyCount, opt => opt.MapFrom(src => src.Properties.Count));
            CreateMap<CreateTypeSaleDto, TypeSale>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TypeName));
            CreateMap<EditTypeSaleDto, TypeSale>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TypeName));
            CreateMap<TypeSale, TypeSaleViewModel>()
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.PropertyCount, opt => opt.MapFrom(src => src.Properties.Count));

            // Improvement
            CreateMap<PropertyImprovement, ImprovementsDto>()
                .ForMember(dest => dest.ImprovementName, opt => opt.MapFrom(src => src.Name));
            CreateMap<CreateImprovementDto, PropertyImprovement>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ImprovementName));
            CreateMap<EditImprovementDto, PropertyImprovement>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ImprovementName));
            CreateMap<PropertyImprovement, ImprovementViewModels>()
                .ForMember(dest => dest.ImprovementName, opt => opt.MapFrom(src => src.Name));

            // AppUser = AgentListDto
            CreateMap<AppUser, AgentListDto>()
                .ForMember(dest => dest.QuantityProperties, opt => opt.MapFrom(src => src.Properties.Count));
            CreateMap<AppUser, AgentsListViewModel>()
                .ForMember(dest => dest.QuantityProperties, opt => opt.MapFrom(src => src.Properties.Count));

            // AppUser = UserListViewModel (admins y developers)
            CreateMap<AppUser, UserListViewModel>()
                .ForMember(dest => dest.Cedula, opt => opt.MapFrom(src => src.IdentityNumber));

            // Agent Profile
            CreateMap<AppUser, AgentProfileViewModel>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.CurrentImage, opt => opt.MapFrom(src => src.PathImg));

            // Agent Property
            CreateMap<Property, AgentPropertyDto>()
                .ForMember(dest => dest.PropertyTypeName, opt => opt.MapFrom(src => src.PropertyType != null ? src.PropertyType.Name : string.Empty))
                .ForMember(dest => dest.TypeSaleName, opt => opt.MapFrom(src => src.TypeSale != null ? src.TypeSale.Name : string.Empty))
                .ForMember(dest => dest.FirstImage, opt => opt.MapFrom(src => src.Images.FirstOrDefault() != null ? src.Images.FirstOrDefault().Path : string.Empty));

            // CreateUserDto = AppUser
            CreateMap<CreateUserDto, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.IdentityNumber, opt => opt.MapFrom(src => src.Cedula))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            // EditUserDto = AppUser
            CreateMap<EditUserDto, AppUser>()
                .ForMember(dest => dest.IdentityNumber, opt => opt.MapFrom(src => src.Cedula));
        }
    }
}
