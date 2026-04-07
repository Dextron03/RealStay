using Application.DTOs.Administrator.Improvements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IImprovementService
    {
        Task<List<ImprovementsDto>> GetAllAsync();
        Task<ImprovementsDto> GetByIdAsync(string id);
        Task CreateAsync(CreateImprovementDto dto);
        Task UpdateAsync(string id, EditImprovementDto dto);
        Task DeleteAsync(string id);
    }
}
