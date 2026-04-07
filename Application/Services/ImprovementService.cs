using Application.DTOs.Administrator.Improvements;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ImprovementService : IImprovementService
    {
        private readonly IGenericRepository<PropertyImprovement> _repository;
        private readonly IMapper _mapper;

        public ImprovementService(IGenericRepository<PropertyImprovement> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ImprovementsDto>> GetAllAsync()
        {
            var improvements = await _repository.GetAllAsync();
            return _mapper.Map<List<ImprovementsDto>>(improvements);
        }

        public async Task<ImprovementsDto> GetByIdAsync(string id)
        {
            var improvements = await _repository.FindAsync(x => x.Id == id);
            var improvement = improvements.FirstOrDefault();
            if (improvement == null) throw new Exception("Mejora no encontrada");
            return _mapper.Map<ImprovementsDto>(improvement);
        }

        public async Task CreateAsync(CreateImprovementDto dto)
        {
            var improvement = _mapper.Map<PropertyImprovement>(dto);
            await _repository.AddAsync(improvement);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateAsync(string id, EditImprovementDto dto)
        {
            var improvements = await _repository.FindAsync(x => x.Id == id);
            var improvement = improvements.FirstOrDefault();
            if (improvement == null) throw new Exception("Mejora no encontrada");
            _mapper.Map(dto, improvement);
            _repository.Update(improvement);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var improvements = await _repository.FindAsync(x => x.Id == id);
            var improvement = improvements.FirstOrDefault();
            if (improvement == null) throw new Exception("Mejora no encontrada");
            _repository.Remove(improvement);
            await _repository.SaveChangesAsync();
        }
    }
}
