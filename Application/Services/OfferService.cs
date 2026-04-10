using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Offer;
using Application.ViewModels.Offers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.Services
{
    public class OfferService : IOfferService
    {
        private readonly IGenericRepository<Offer> _genericRepository;

        private readonly IMapper _mapper;

        public OfferService(IGenericRepository<Offer> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        public async Task CreateAsync(SaveOfferViewModel vm){
            var offer = _mapper.Map<Offer>(vm);

            offer.Status = OfferStatus.Pending.ToString();

            await _genericRepository.AddAsync(offer);
            await _genericRepository.SaveChangesAsync();
        }

        public async Task<List<OfferViewModel>> GetByPropertyAsync(string propertyId){
            var offer = await _genericRepository
                .FindAsync(o => o.PropertyId == propertyId);

            return _mapper.Map<List<OfferViewModel>>(offer.OrderBy(o => o.DateRegistration));
        }

        public async Task<List<OfferViewModel>> GetByUserAsync(string userId){
            var offers = await _genericRepository
                .FindAsync(o => o.UserId == userId);
            
            return _mapper.Map<List<OfferViewModel>>(offers.OrderBy(o => o.DateRegistration));
            
        }

        public async Task AcceptAsync(string offerId){
            var offer = await _genericRepository.GetByIdAsync(offerId);
            
            offer.Status = OfferStatus.Accepted.ToString();
            
            _genericRepository.Update(offer);
            await _genericRepository.SaveChangesAsync();
        }

        public async Task RejectAsync(string offerId){
            var offer = await _genericRepository.GetByIdAsync(offerId);
            
            offer.Status = OfferStatus.Rejected.ToString();
            
            _genericRepository.Update(offer);
            await _genericRepository.SaveChangesAsync();
        }
    }
}