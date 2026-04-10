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
    public class WishListService 
    {
        private readonly IGenericRepository<WishList> _genericRepository;
        private readonly IMapper _mapper;

        public WishListService(IGenericRepository<WishList> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        public async Task AddToWishListAsync(string clientId, string propertyId)
        {
            /* _genericRepository.AddAsync(); */
        }

        public async Task RemoveFromWishListAsync(string clientId, string propertyId)
        {

        }

/*         public async Task<List<PropertyViewModel>> GetWishListAsync(string clientId)
        {
            
        } */
    }
}