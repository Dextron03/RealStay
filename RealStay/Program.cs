using Application;
using Infrastructure.Persistence;
using Infrastructure.Identity;
using Infrastructure.Identity.Seeds;
using Shared.Services;
using Shared.Settings;

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