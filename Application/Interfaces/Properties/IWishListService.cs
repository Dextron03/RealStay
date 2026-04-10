using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.ViewModels.Properties;

namespace Application.Interfaces.Properties
{
    public interface IWishListService
    {
        Task AddToWishListAsync(string clientId, string propertyId);
        Task RemoveFromWishListAsync(string clientId, string propertyId);
        Task<List<PropertyViewModel>> GetWishListAsync(string clientId);
    }
}