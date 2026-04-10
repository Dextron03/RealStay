using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Properties;
using Application.ViewModels.Properties;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class WishListService : IWishListService
    {
        private readonly IGenericRepository<WishList> _genericRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public WishListService(IGenericRepository<WishList> genericRepository, IPropertyRepository propertyRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        public async Task AddToWishListAsync(string clientId, string propertyId)
        {
            var existing = await _genericRepository.FindAsync(w => w.ClientId == clientId && w.PropertyId == propertyId);
            if (existing.Any())
                return;

            var wishList = new WishList
            {
                ClientId = clientId,
                PropertyId = propertyId
            };

            await _genericRepository.AddAsync(wishList);
        }

        public async Task RemoveFromWishListAsync(string clientId, string propertyId)
        {
            var entries = await _genericRepository.FindAsync(w => w.ClientId == clientId && w.PropertyId == propertyId);
            var entry = entries.FirstOrDefault();
            if (entry == null)
                return;

            _genericRepository.Remove(entry);
            await _genericRepository.SaveChangesAsync();
        }

        public async Task<List<PropertyViewModel>> GetWishListAsync(string clientId)
        {
            var wishListEntries = await _genericRepository.FindAsync(w => w.ClientId == clientId);

            var result = new List<PropertyViewModel>();
            foreach (var entry in wishListEntries)
            {
                var property = await _propertyRepository.GetByIdWithDetailsAsync(entry.PropertyId);
                if (property != null)
                    result.Add(_mapper.Map<PropertyViewModel>(property));
            }

            return result;
        }
    }
}