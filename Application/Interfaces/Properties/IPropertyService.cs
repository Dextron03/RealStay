using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.ViewModels.Properties;

namespace Application.Interfaces.Properties
{
    public interface IPropertyService
    {
        Task<List<PropertyViewModel>> GetAllAsync();
        Task<List<PropertyViewModel>> GetByAgentAsync(string agentId);
        Task<PropertyViewModel> GetByIdAsync(string id);
        Task CreateAsync(SavePropertyViewModel vm);
        Task UpdateAsync(string id, SavePropertyViewModel vm);
        Task DeleteAsync(string id);
        Task<List<PropertyViewModel>> FilterAsync(PropertyFilterViewModel filters);
    }
}