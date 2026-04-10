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
            services.AddTransient<IAdminDashboardService, AdminDashboardService>();
            services.AddTransient<IAdminUserService, AdminUserService>();
            services.AddTransient<IAgentService, AgentService>();
            services.AddTransient<IImprovementService, ImprovementService>();
            services.AddTransient<IPropertyTypeService, PropertyTypeService>();
            services.AddTransient<ISaleTypeService, SaleTypeService>();
            #endregion
        }
    }
}