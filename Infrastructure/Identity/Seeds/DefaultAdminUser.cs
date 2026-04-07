using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Seeds
{
    public static class DefaultAdminUser
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager)
        {
            // Hacemos un super usuario administrador
            var defaultUser = new AppUser
            {
                UserName = "Therian69",
                Email = "michelle.clau12@gmail.com",
                FirstName = "Claudia",
                LastName = "Zapata",
                IdentityNumber = "00000000000",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };

            // Verficamos si ya existe algun usuario con este correo
            var user = await userManager.FindByEmailAsync(defaultUser.Email);

            if(user == null)
            {
                // Lo creamos en la base de datos con una contraseña segura
                await userManager.CreateAsync(defaultUser, "123Pa$$word!");

                await userManager.AddToRoleAsync(defaultUser, Role.Administrator.ToString());
            }
        }
    }
}