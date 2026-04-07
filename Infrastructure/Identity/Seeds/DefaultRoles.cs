using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            // Verificamos si los roles ya existen en la base de datos para no duplicalos 
            if(!await roleManager.RoleExistsAsync(Role.Administrator.ToString())) 
                await roleManager.CreateAsync(new IdentityRole(Role.Administrator.ToString()));

            if(!await roleManager.RoleExistsAsync(Role.Agent.ToString()))
                await roleManager.CreateAsync(new IdentityRole(Role.Agent.ToString()));

            if(!await roleManager.RoleExistsAsync(Role.Client.ToString()))
                await roleManager.CreateAsync(new IdentityRole(Role.Client.ToString()));

            if(!await roleManager.RoleExistsAsync(Role.Developer.ToString()))
                await roleManager.CreateAsync(new IdentityRole(Role.Developer.ToString()));
        }
    }
}
