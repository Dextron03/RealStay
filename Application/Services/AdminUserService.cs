using Application.DTOs.Administrator.User;
using Application.Interfaces;
using Application.Interfaces.Agent;
using Application.ViewModels.Administrator.Agent;
using Application.ViewModels.Administrator.User;
using Domain.Enums;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AdminUserService : IAdminUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAgentService _agentService;

        public AdminUserService(UserManager<AppUser> userManager, IAgentService agentService)
        {
            _userManager = userManager;
            _agentService = agentService;
        }

        public async Task<List<UserListViewModel>> GetAllAdminsAsync()
        {
            var users = await _userManager.GetUsersInRoleAsync(Role.Administrator.ToString());
            return users.Select(u => new UserListViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName ?? string.Empty,
                Cedula = u.IdentityNumber,
                Email = u.Email ?? string.Empty,
                IsActive = u.IsActive
            }).ToList();
        }

        public async Task<List<UserListViewModel>> GetAllDevelopersAsync()
        {
            var users = await _userManager.GetUsersInRoleAsync(Role.Developer.ToString());
            return users.Select(u => new UserListViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName ?? string.Empty,
                Cedula = u.IdentityNumber,
                Email = u.Email ?? string.Empty,
                IsActive = u.IsActive
            }).ToList();
        }

        public async Task<List<AgentsListViewModel>> GetAllAgentsAsync()
        {
            var agentsInRole = await _userManager.GetUsersInRoleAsync(Role.Agent.ToString());
            var agentIds = agentsInRole.Select(a => a.Id).ToList();

            var users = await _userManager.Users
                .Include(u => u.Properties)
                .Where(u => agentIds.Contains(u.Id))
                .ToListAsync();

            return users.Select(u => new AgentsListViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                QuantityProperties = u.Properties.Count,
                Email = u.Email ?? string.Empty,
                IsActive = u.IsActive
            }).ToList();
        }

        public async Task CreateAdminAsync(CreateUserDto dto)
        {
            var user = new AppUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                IdentityNumber = dto.Cedula,
                Email = dto.Email,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Error al crear usuario: {errors}");
            }

            await _userManager.AddToRoleAsync(user, Role.Administrator.ToString());
        }

        public async Task CreateDeveloperAsync(CreateUserDto dto)
        {
            var user = new AppUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                IdentityNumber = dto.Cedula,
                Email = dto.Email,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Error al crear usuario: {errors}");
            }

            await _userManager.AddToRoleAsync(user, Role.Developer.ToString());
        }

        public async Task UpdateUserAsync(string id, EditUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new Exception("Usuario no encontrado");

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.UserName = dto.UserName;
            user.IdentityNumber = dto.Cedula;
            user.Email = dto.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Error al actualizar usuario: {errors}");
            }

            if (!string.IsNullOrEmpty(dto.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, dto.Password);
                if (!passwordResult.Succeeded)
                {
                    var errors = string.Join(", ", passwordResult.Errors.Select(e => e.Description));
                    throw new Exception($"Error al actualizar la contraseña {errors}");
                }
            }
        }

        public async Task ToggleActiveAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new Exception("Usuario no encontrado");

            user.IsActive = !user.IsActive;
            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteAgentAsync(string id)
        {
            var user = await _userManager.Users
                .Include(u => u.Properties)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) throw new Exception("Agente no encontrado");

            var propertyIds = user.Properties.Select(p => p.Id).ToList();
            foreach (var propertyId in propertyIds)
            {
                await _agentService.DeletePropertyAsync(propertyId, user.Id);
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Error al eliminar al agente {errors}");
            }
        }
    }
}