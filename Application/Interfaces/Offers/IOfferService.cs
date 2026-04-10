using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.ViewModels.Offers;

namespace Application.Interfaces.Offer
{
    public interface IOfferService
    {
        Task CreateAsync(SaveOfferViewModel vm);
        Task<List<OfferViewModel>> GetByPropertyAsync(string propertyId);
        Task<List<OfferViewModel>> GetByUserAsync(string userId);
        Task AcceptAsync(string offerId);
        Task RejectAsync(string offerId);

    }
}