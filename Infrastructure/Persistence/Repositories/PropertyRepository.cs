using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Identity.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PropertyRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Property>> GetAllWithDetailsAsync()
        {
            return await _dbContext.Properties
                .Include(p => p.PropertyType)
                .Include(p => p.TypeSale)
                .Include(p => p.Images)
                .Include(p => p.Improvements)
                    .ThenInclude(pi => pi.Improvement) //  que es lo que el repositorio genérico no puede hacer. Por eso necesitas el repositorio específico.
                .ToListAsync();
        }

        public async Task<List<Property>> GetByAgentDetailsAsync(string agentId)
        {
            return await _dbContext.Properties
                .Where(p => p.AgentId == agentId)
                .Include(p => p.PropertyType)
                .Include(p => p.TypeSale)
                .Include(p => p.Images)
                .Include(p => p.Improvements)
                    .ThenInclude(pi => pi.Improvement)
                .ToListAsync();
        }

        public async Task<Property> GetByIdWithDetailsAsync(string id)
        {
            return await _dbContext.Properties.
                Where(p => p.Id == id)
                .Include(p => p.PropertyType)
                .Include(p => p.TypeSale)
                .Include(p => p.Images)
                .Include(p => p.Improvements)
                    .ThenInclude(pi => pi.Improvement)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Property>> FilterAsync(string? typeSaleName, string? propertyTypeId, decimal? minPrice, decimal? maxPrice, int? rooms, int? bathrooms)
        {
            var query = _dbContext.Properties
                .Include(p => p.PropertyType)
                .Include(p => p.TypeSale)
                .Include(p => p.Images)
                .Include(p => p.Improvements)
                    .ThenInclude(pi => pi.Improvement)
                .AsQueryable();

            if (!string.IsNullOrEmpty(typeSaleName))
                query = query.Where(p => p.TypeSale!.Name == typeSaleName);

            if (!string.IsNullOrEmpty(propertyTypeId))
                query = query.Where(p => p.PropertyTypeId == propertyTypeId);

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            if (rooms.HasValue)
                query = query.Where(p => p.NumberRooms == rooms.Value);

            if (bathrooms.HasValue)
                query = query.Where(p => p.NumberBaths == bathrooms.Value);

            return await query.ToListAsync();
        }

    }
}