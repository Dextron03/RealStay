using Application.DTOs.Administrator.Sales;
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
    public class SaleTypeService : ISaleTypeService
    {
        private readonly IGenericRepository<TypeSale> _repository;
        private readonly IMapper _mapper;

        public SaleTypeService(IGenericRepository<TypeSale> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<SalesTypeDto>> GetAllAsync()
        {
            var types = await _repository.GetAllAsync(x => x.Properties);
            return _mapper.Map<List<SalesTypeDto>>(types);
        }

        public async Task<SalesTypeDto> GetByIdAsync(string id)
        {
            var types = await _repository.FindAsync(x => x.Id == id, x => x.Properties);
            var type = types.FirstOrDefault();
            if (type == null) throw new Exception("Tipo de venta no encontrado");
            return _mapper.Map<SalesTypeDto>(type);
        }

        public async Task CreateAsync(CreateTypeSaleDto dto)
        {
            var type = _mapper.Map<TypeSale>(dto);
            await _repository.AddAsync(type);
            await _repository.SaveChangesAsync();
        }

        public async Task UpdateAsync(string id, EditTypeSaleDto dto)
        {
            var types = await _repository.FindAsync(x => x.Id == id);
            var type = types.FirstOrDefault();
            if (type == null) throw new Exception("Tipo de venta no encontrado");
            _mapper.Map(dto, type);
            _repository.Update(type);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var types = await _repository.FindAsync(x => x.Id == id, x => x.Properties);
            var type = types.FirstOrDefault();
            if (type == null) throw new Exception("Tipo de venta no encontrado");
            _repository.Remove(type);
            await _repository.SaveChangesAsync();
        }
    }
}
