using Infrastructure.Persistence;
using Infrastructure.Identity;

namespace RealStay
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // 2) Infraestructura (Identity + EF, etc.)
            builder.Services.AddPersistenceLayer(builder.Configuration);
            builder.Services.AddIdentityInfrastructure(builder.Configuration);


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

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
