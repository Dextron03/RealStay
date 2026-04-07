using Application.DTOs.Administrator.User;
using Application.ViewModels.Administrator.Agent;
using Application.ViewModels.Administrator.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAdminUserService
    {
        Task<List<UserListViewModel>> GetAllAdminsAsync();
        Task<List<UserListViewModel>> GetAllDevelopersAsync();
        Task<List<AgentsListViewModel>> GetAllAgentsAsync();
        Task CreateAdminAsync(CreateUserDto dto);
        Task CreateDeveloperAsync(CreateUserDto dto);
        Task UpdateUserAsync(string id, EditUserDto dto);
        Task ToggleActiveAsync(string id);
        Task DeleteAgentAsync(string id);
    }
}
