using Application.Interfaces;
using Application.Interfaces.Message;
using Application.Interfaces.Offer;
using Application.Interfaces.Properties;
using Application.Mappings;
using Application.Services;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(GeneralProfile).Assembly);

            #region Services
            services.AddTransient<IPropertyService, PropertyService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<IPropertyTypeService, PropertyTypeService>();
            services.AddTransient<IWishListService, WishListService>();
            services.AddTransient<IOfferService, OfferService>();
            services.AddTransient<IImprovementService, ImprovementService>();
            services.AddTransient<ISaleTypeService, SaleTypeService>();
            // Por qué AddTransient: estos servicios no guardan estado entre requests, así que Transient es lo más seguro. Si usaras AddScoped también funcionaría (mismo ciclo de vida
            // que el DbContext), pero Transient es suficiente aquí.
            #endregion

            return services;
        }
    }
}
