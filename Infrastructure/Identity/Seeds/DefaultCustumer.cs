using Domain.Enums;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Seeds
{
    public static class DefaultCustumer
    {
                public static async Task SeedAsync(UserManager<AppUser> userManager)
        {
            var defaultUser = new AppUser
            {
                UserName = "Sketty",
                Email = "ambrousecream@gmail.com",
                FirstName = "Scarlet",
                LastName = "Maris",
                IdentityNumber = "11111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };

            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if(user == null)
            {
                await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                await userManager.AddToRoleAsync(defaultUser, Role.Client.ToString());
            }
        }
    }
}        