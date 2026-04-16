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
using Application.ViewModels.Messages;
using Application.ViewModels.Offers;
using Application.ViewModels.Properties;
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

            // Property -> PropertyViewModel
            CreateMap<Property, PropertyViewModel>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.PropertyCode))
                .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.NumberRooms))
                .ForMember(dest => dest.Bathrooms, opt => opt.MapFrom(src => src.NumberBaths))
                .ForMember(dest => dest.Meters, opt => opt.MapFrom(src => (double)src.Meters))
                .ForMember(dest => dest.PropertyTypeName, opt => opt.MapFrom(src => src.PropertyType != null ? src.PropertyType.Name : string.Empty))
                .ForMember(dest => dest.TypeSaleName, opt => opt.MapFrom(src => src.TypeSale != null ? src.TypeSale.Name : string.Empty))
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images.Select(i => i.Path).ToList()))
                .ForMember(dest => dest.ImprovementNames, opt => opt.MapFrom(src => src.Improvements.Where(i => i.Improvement != null).Select(i => i.Improvement!.Name).ToList()))
                .ForMember(dest => dest.AgentName, opt => opt.Ignore())
                .ForMember(dest => dest.AgentPhone, opt => opt.Ignore());

            // SavePropertyViewModel -> Property
            CreateMap<SavePropertyViewModel, Property>()
                .ForMember(dest => dest.NumberRooms, opt => opt.MapFrom(src => src.Rooms))
                .ForMember(dest => dest.NumberBaths, opt => opt.MapFrom(src => src.Bathrooms))
                .ForMember(dest => dest.Meters, opt => opt.MapFrom(src => (int)src.Meters))
                .ForMember(dest => dest.PropertyCode, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Improvements, opt => opt.Ignore())
                .ForMember(dest => dest.Offers, opt => opt.Ignore())
                .ForMember(dest => dest.Messages, opt => opt.Ignore())
                .ForMember(dest => dest.RegistrationDate, opt => opt.Ignore());

            // Offer -> OFferViewModel
            CreateMap<Offer, OfferViewModel>();

            //  SaveOfferViewModel → Offer
            CreateMap<SaveOfferViewModel, Offer>()
                .ForMember(dest => dest.OfferAmount, opt => opt.MapFrom(src => src.OfferAmount))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.PropertyId, opt => opt.MapFrom(src => src.PropertyId));

            //  Message → MessageViewModel 
            CreateMap<Message, MessageViewModel>();

            // SaveMessageViewModel → Message
            CreateMap<SaveMessageViewModel, Message>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.DateSend, opt => opt.Ignore());
        }
    }
}
