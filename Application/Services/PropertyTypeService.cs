using Application.DTOs.Administrator.Properties;
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
    public class PropertyTypeService : IPropertyTypeService
    {
        private readonly IGenericRepository<PropertyType> _repository;
        private readonly IMapper _mapper;

        public PropertyTypeService(IGenericRepository<PropertyType> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PropertiesDto>> GetAllAsync()
        {
            var types = await _repository.GetAllAsync(x => x.Properties);
            return _mapper.Map<List<PropertiesDto>>(types);
        }

        public async Task<PropertiesDto> GetByIdAsync(string id)
        {
            var types = await _repository.FindAsync(x => x.Id == id, x => x.Properties);
            var type = types.FirstOrDefault();
            if (type == null) throw new Exception("Tipo de propiedad no encontrado");
            return _mapper.Map<PropertiesDto>(type);
        }

        public async Task CreateAsync(CreatePropertyDto dto)
        {
            var type = _mapper.Map<PropertyType>(dto);
            await _repository.AddAsync(type);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateAsync(string id, EditPropertyDto dto)
        {
            var types = await _repository.FindAsync(x => x.Id == id);
            var type = types.FirstOrDefault();
            if (type == null) throw new Exception("Tipo de propiedad no encontrado");
            _mapper.Map(dto, type);
            _repository.Update(type);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var types = await _repository.FindAsync(x => x.Id == id, x => x.Properties);
            var type = types.FirstOrDefault();
            if (type == null) throw new Exception("Tipo de propiedad no encontrado");
            _repository.Remove(type);
            await _repository.SaveChangesAsync();
        }
    }
}
