using Application.Interfaces;
using Application.Interfaces.Agent;
using Application.Interfaces.Properties;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ServiceRegistration));

            #region Services
            services.AddScoped<IAdminUserService, AdminUserService>();
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IImprovementService, ImprovementService>();
            services.AddScoped<IPropertyTypeService, PropertyTypeService>();
            services.AddScoped<ISaleTypeService, SaleTypeService>();
            #endregion
        }
    }
}