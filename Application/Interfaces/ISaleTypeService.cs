using Application.DTOs.Administrator.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISaleTypeService
    {
        Task<List<SalesTypeDto>> GetAllAsync();
        Task<SalesTypeDto> GetByIdAsync(string id);
        Task CreateAsync(CreateTypeSaleDto dto);
        Task UpdateAsync(string id, EditTypeSaleDto dto);
        Task DeleteAsync(string id);
    }
}
