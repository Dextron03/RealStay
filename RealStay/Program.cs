using Application;
using Infrastructure.Persistence;
using Infrastructure.Identity;
using Infrastructure.Identity.Seeds;
using Shared.Services;
using Shared.Settings;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace RealStay
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddApplicationLayer();

            // 2) Infraestructura (Identity + EF, etc.)
            builder.Services.AddPersistenceLayer(builder.Configuration);
            builder.Services.AddIdentityInfrastructure(builder.Configuration);

            // Email
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddTransient<IEmailService, EmailService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Forzar cultura invariante para que el model binding de decimal
            // siempre use punto como separador decimal, sin importar la cultura del SO.
            var invariantCulture = CultureInfo.InvariantCulture;
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(invariantCulture),
                SupportedCultures = new[] { invariantCulture },
                SupportedUICultures = new[] { invariantCulture }
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Index}/{id?}")
                .WithStaticAssets();

            await app.RunIdentitySeedsAsync();

            app.Run();
        }
    }
}