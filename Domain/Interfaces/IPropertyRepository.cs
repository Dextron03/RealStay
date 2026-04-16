using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPropertyRepository
    {
        Task<List<Property>> GetAllWithDetailsAsync();
        Task<List<Property>> GetByAgentDetailsAsync(string agentId);
        Task<Property> GetByIdWithDetailsAsync(string id);
        Task<List<Property>> FilterAsync(string? typeSaleName, string? propertyTypeId, decimal? minPrice, decimal? maxPrice, int? rooms, int? bathrooms);
    }
}