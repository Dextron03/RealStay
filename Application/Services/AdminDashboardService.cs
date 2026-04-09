using Application.DTOs.Administrator.Dashboards;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericRepository<Property> _propertyRepository;

        public AdminDashboardService(UserManager<AppUser> userManager, IGenericRepository<Property> propertyRepository)
        {
            _userManager = userManager;
            _propertyRepository = propertyRepository;
        }

        public async Task<AdminDashboarDto> GetDashboardDataAsync()
        {
            var properties = await _propertyRepository.GetAllAsync();
            var agents = await _userManager.GetUsersInRoleAsync("Agent");
            var clients = await _userManager.GetUsersInRoleAsync("Client");
            var developers = await _userManager.GetUsersInRoleAsync("Developer");

            return new AdminDashboarDto
            {
                AvailableProperties = properties.Count(p => p.Status == PropertyStatus.Available.ToString()),
                SoldProperties = properties.Count(p => p.Status == PropertyStatus.Sold.ToString()),
                ActiveAgents = agents.Count(u => u.IsActive),
                InactiveAgents = agents.Count(u => !u.IsActive),
                ActiveClients = clients.Count(u => u.IsActive),
                InactiveClients = clients.Count(u => !u.IsActive),
                ActiveDevelopers = developers.Count(u => u.IsActive),
                InactiveDevelopers = developers.Count(u => !u.IsActive)
            };
        }
    }
}
