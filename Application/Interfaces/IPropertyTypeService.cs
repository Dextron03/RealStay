using Application.DTOs.Administrator.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPropertyTypeService
    {
        Task<List<PropertiesDto>> GetAllAsync();
        Task<PropertiesDto> GetByIdAsync(string id);
        Task CreateAsync(CreatePropertyDto dto);
        Task UpdateAsync(string id, EditPropertyDto dto);
        Task DeleteAsync(string id);
    }
}
