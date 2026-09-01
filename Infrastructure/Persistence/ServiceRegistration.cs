using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;
using Infrastructure.Identity.Contexts;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            // Si "UseInMemoryDatabase" esta en true (ver appsettings.Development.json)
            // se levanta una base de datos en memoria; util para demos donde solo
            // se quieren mostrar las interfaces sin instalar SQL Server.
            var useInMemory = configuration.GetValue<bool>("UseInMemoryDatabase");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (useInMemory)
                {
                    options.UseInMemoryDatabase("RealStayDb");
                }
                else
                {
                    options.UseSqlServer(configuration
                        .GetConnectionString("DefaultConnection"),
                        m => m.MigrationsAssembly(typeof(ApplicationDbContext)
                        .Assembly.FullName));
                }
            });

            #region  Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IPropertyRepository, PropertyRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();
            #endregion
            // Por qué AddTransient: estos servicios no guardan estado entre requests, así que Transient es lo más seguro. Si usaras AddScoped también funcionaría (mismo ciclo de vida
            // que el DbContext), pero Transient es suficiente aquí.
        }
    }
}