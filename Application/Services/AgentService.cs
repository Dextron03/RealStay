using Application.DTOs.Agent;
using Application.Interfaces.Agent;
using Application.Interfaces.Offer;
using Application.ViewModels.Agent.Properties;
using Application.ViewModels.Agent.Profile;
using Application.ViewModels.Messages;
using Application.ViewModels.Offers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AgentService : IAgentService
    {
        private readonly IGenericRepository<Property> _propertyRepository;
        private readonly IGenericRepository<PropertyType> _propertyTypeRepository;
        private readonly IGenericRepository<TypeSale> _typeSaleRepository;
        private readonly IGenericRepository<PropertyImprovement> _improvementRepository;
        private readonly IGenericRepository<PropertyImage> _propertyImageRepository;
        private readonly IGenericRepository<PropertyImprovements> _propertyImprovementsRepository;
        private readonly IGenericRepository<Offer> _offerRepository;
        private readonly IGenericRepository<Message> _messageRepository;
        private readonly IMessageRepository _messageRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public AgentService(
            IGenericRepository<Property> propertyRepository,
            IGenericRepository<PropertyType> propertyTypeRepository,
            IGenericRepository<TypeSale> typeSaleRepository,
            IGenericRepository<PropertyImprovement> improvementRepository,
            IGenericRepository<PropertyImage> propertyImageRepository,
            IGenericRepository<PropertyImprovements> propertyImprovementsRepository,
            IGenericRepository<Offer> offerRepository,
            IGenericRepository<Message> messageRepository,
            IMessageRepository messageRepo,
            UserManager<AppUser> userManager,
            IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _propertyTypeRepository = propertyTypeRepository;
            _typeSaleRepository = typeSaleRepository;
            _improvementRepository = improvementRepository;
            _propertyImageRepository = propertyImageRepository;
            _propertyImprovementsRepository = propertyImprovementsRepository;
            _offerRepository = offerRepository;
            _messageRepository = messageRepository;
            _messageRepo = messageRepo;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<AgentPropertyDto>> GetPropertiesByAgentIdAsync(string agentId)
        {
            var properties = await _propertyRepository.FindAsync(
                p => p.AgentId == agentId,
                p => p.PropertyType,
                p => p.TypeSale,
                p => p.Images);

            return properties.Select(p => new AgentPropertyDto
            {
                Id = p.Id,
                PropertyCode = p.PropertyCode,
                Name = p.Name,
                PropertyTypeId = p.PropertyTypeId,
                PropertyTypeName = p.PropertyType?.Name ?? string.Empty,
                TypeSaleId = p.TypeSaleId,
                TypeSaleName = p.TypeSale?.Name ?? string.Empty,
                Price = p.Price,
                NumberRooms = p.NumberRooms,
                NumberBaths = p.NumberBaths,
                Meters = p.Meters,
                Status = p.Status,
                Location = p.Location,
                Description = p.Description,
                FirstImage = p.Images.FirstOrDefault()?.Path ?? string.Empty,
                ImageUrls = p.Images.Select(i => i.Path ?? string.Empty).ToList()
            }).ToList();
        }

        public async Task<AgentPropertyDto> GetPropertyByIdAsync(string propertyId, string agentId)
        {
            var properties = await _propertyRepository.FindAsync(
                p => p.Id == propertyId && p.AgentId == agentId,
                p => p.PropertyType,
                p => p.TypeSale,
                p => p.Images,
                p => p.Improvements);

            var property = properties.FirstOrDefault();
            if (property == null)
                throw new Exception("Propiedad no encontrada");

            return new AgentPropertyDto
            {
                Id = property.Id,
                PropertyCode = property.PropertyCode,
                Name = property.Name,
                PropertyTypeId = property.PropertyTypeId,
                PropertyTypeName = property.PropertyType?.Name ?? string.Empty,
                TypeSaleId = property.TypeSaleId,
                TypeSaleName = property.TypeSale?.Name ?? string.Empty,
                Price = property.Price,
                NumberRooms = property.NumberRooms,
                NumberBaths = property.NumberBaths,
                Meters = property.Meters,
                Status = property.Status,
                Description = property.Description,
                Location = property.Location,
                FirstImage = property.Images.FirstOrDefault()?.Path ?? string.Empty,
                ImageUrls = property.Images.Select(i => i.Path ?? string.Empty).ToList(),
                ImprovementNames = property.Improvements.Select(i => i.Improvement?.Name ?? string.Empty).ToList()
            };
        }

        public async Task<string> CreatePropertyAsync(CreateAgentPropertyViewModel model, string agentId)
        {
            var propertyTypes = await _propertyTypeRepository.GetAllAsync();
            var typeSales = await _typeSaleRepository.GetAllAsync();
            var improvements = await _improvementRepository.GetAllAsync();

            if (!propertyTypes.Any())
                throw new Exception("No existen tipos de propiedades creadas. Contacte al administrador.");
            if (!typeSales.Any())
                throw new Exception("No existen tipos de ventas creadas. Contacte al administrador.");
            if (!improvements.Any())
                throw new Exception("No existen mejoras creadas. Contacte al administrador.");

            var property = new Property
            {
                Name = $"Propiedad - {DateTime.Now:yyyyMMddHHmmss}",
                PropertyCode = GenerateUniqueCode(),
                PropertyTypeId = model.PropertyTypeId,
                TypeSaleId = model.TypeSaleId,
                Price = model.Price,
                Description = model.Description,
                Meters = model.Meters,
                NumberRooms = model.NumberRooms,
                NumberBaths = model.NumberBaths,
                Location = model.Location,
                AgentId = agentId,
                Status = PropertyStatus.Available.ToString(),
                RegistrationDate = DateTime.Now
            };

            await _propertyRepository.AddAsync(property);

            foreach (var image in model.Images)
            {
                var imagePath = await SaveImageAsync(image);
                var propertyImage = new PropertyImage
                {
                    PropertyId = property.Id,
                    Path = imagePath
                };
                await _propertyImageRepository.AddAsync(propertyImage);
            }

            foreach (var improvementId in model.ImprovementIds)
            {
                var propertyImprovement = new PropertyImprovements
                {
                    PropertyId = property.Id,
                    ImprovementId = improvementId
                };
                await _propertyImprovementsRepository.AddAsync(propertyImprovement);
            }

            await _propertyRepository.SaveChangesAsync();
            return property.Id;
        }

        public async Task UpdatePropertyAsync(string propertyId, string agentId, EditAgentPropertyViewModel model)
        {
            var properties = await _propertyRepository.FindAsync(
                p => p.Id == propertyId && p.AgentId == agentId,
                p => p.Images,
                p => p.Improvements);

            var property = properties.FirstOrDefault();
            if (property == null)
                throw new Exception("Propiedad no encontrada");

            property.PropertyTypeId = model.PropertyTypeId;
            property.TypeSaleId = model.TypeSaleId;
            property.Price = model.Price;
            property.Description = model.Description;
            property.Meters = model.Meters;
            property.NumberRooms = model.NumberRooms;
            property.NumberBaths = model.NumberBaths;
            property.Location = model.Location;

            _propertyRepository.Update(property);

            if (model.Images != null && model.Images.Any())
            {
                foreach (var existingImage in property.Images)
                {
                    _propertyImageRepository.Remove(existingImage);
                }

                foreach (var image in model.Images)
                {
                    var imagePath = await SaveImageAsync(image);
                    var propertyImage = new PropertyImage
                    {
                        PropertyId = property.Id,
                        Path = imagePath
                    };
                    await _propertyImageRepository.AddAsync(propertyImage);
                }
            }

            var existingImprovements = property.Improvements.ToList();
            foreach (var imp in existingImprovements)
            {
                _propertyImprovementsRepository.Remove(imp);
            }

            foreach (var improvementId in model.ImprovementIds)
            {
                var propertyImprovement = new PropertyImprovements
                {
                    PropertyId = property.Id,
                    ImprovementId = improvementId
                };
                await _propertyImprovementsRepository.AddAsync(propertyImprovement);
            }

            await _propertyRepository.SaveChangesAsync();
        }

        public async Task DeletePropertyAsync(string propertyId, string agentId)
        {
            var properties = await _propertyRepository.FindAsync(
                p => p.Id == propertyId && p.AgentId == agentId,
                p => p.Images,
                p => p.Improvements,
                p => p.Offers,
                p => p.Messages);

            var property = properties.FirstOrDefault();
            if (property == null)
                throw new Exception("Propiedad no encontrada");

            foreach (var image in property.Images)
            {
                if (!string.IsNullOrEmpty(image.Path) && File.Exists(image.Path))
                {
                    File.Delete(image.Path);
                }
                _propertyImageRepository.Remove(image);
            }

            foreach (var improvement in property.Improvements)
            {
                _propertyImprovementsRepository.Remove(improvement);
            }

            _propertyRepository.Remove(property);
            await _propertyRepository.SaveChangesAsync();
        }

        public async Task<AgentProfileDto> GetAgentProfileAsync(string agentId)
        {
            var user = await _userManager.FindByIdAsync(agentId);
            if (user == null)
                throw new Exception("Agente no encontrado");

            return new AgentProfileDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber ?? string.Empty,
                PathImg = user.PathImg,
                Email = user.Email ?? string.Empty
            };
        }

        public async Task UpdateAgentProfileAsync(string agentId, UpdateAgentProfileDto model)
        {
            var user = await _userManager.FindByIdAsync(agentId);
            if (user == null)
                throw new Exception("Agente no encontrado");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;

            if (model.Image != null && model.Image.Length > 0)
            {
                if (!string.IsNullOrEmpty(user.PathImg) && File.Exists(user.PathImg))
                {
                    File.Delete(user.PathImg);
                }
                user.PathImg = await SaveImageAsync(model.Image);
            }

            await _userManager.UpdateAsync(user);
        }

        public async Task<List<AgentProfileDto>> GetAllAgentsAsync()
        {
            var users = await _userManager.GetUsersInRoleAsync("Agent");
            var agents = users.Where(u => u.IsActive).ToList();

            return agents.Select(u => new AgentProfileDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email ?? string.Empty,
                Phone = u.PhoneNumber ?? string.Empty,
                PathImg = u.PathImg ?? string.Empty
            }).ToList();
        }

        public async Task<List<AgentProfileDto>> SearchAgentsByNameAsync(string name)
        {
            var agents = await GetAllAgentsAsync();
            if (string.IsNullOrEmpty(name)) return agents;

            return agents.Where(a =>
                a.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
                a.LastName.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private async Task<string> SaveImageAsync(IFormFile image)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "properties");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/uploads/properties/{fileName}";
        }

        private string GenerateUniqueCode()
        {
            Random random = new Random();
            string code;
            bool exists;

            do
            {
                code = random.Next(100000, 999999).ToString();
                exists = _propertyRepository.FindAsync(p => p.PropertyCode == code).Result.Any();
            } while (exists);

            return code;
        }

        public async Task<List<ChatViewModel>> GetChatsForAgentAsync(string agentId)
        {
            var chats = await _messageRepo.GetChatsForUserAsync(agentId);

            return chats.Select(m => new ChatViewModel
            {
                Id = m.Id,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                PropertyId = m.PropertyId,
                LastMessage = m.Content,
                DateLastMessage = m.DateSend
            }).ToList();
        }

        public async Task<List<MessageViewModel>> GetMessagesByPropertyAsync(string propertyId, string clientId, string agentId)
        {
            var messages = await _messageRepository
                .FindAsync(m =>
                    m.PropertyId == propertyId &&
                    ((m.SenderId == clientId && m.ReceiverId == agentId) ||
                    (m.SenderId == agentId && m.ReceiverId == clientId)));

            return _mapper.Map<List<MessageViewModel>>(messages.OrderBy(m => m.DateSend));
        }

        public async Task SendMessageAsync(SaveMessageViewModel vm)
        {
            var message = _mapper.Map<Message>(vm);
            await _messageRepository.AddAsync(message);
            await _messageRepository.SaveChangesAsync();
        }

        public async Task<List<OfferViewModel>> GetOffersByAgentAsync(string agentId)
        {
            var offers = await _offerRepository.FindAsync(
                o => o.Property.AgentId == agentId,
                o => o.Property);

            var result = new List<OfferViewModel>();
            foreach (var offer in offers)
            {
                var user = await _userManager.FindByIdAsync(offer.UserId);
                result.Add(new OfferViewModel
                {
                    Id = offer.Id,
                    PropertyId = offer.PropertyId,
                    PropertyName = offer.Property?.Name ?? string.Empty,
                    UserId = offer.UserId,
                    UserName = user != null ? $"{user.FirstName} {user.LastName}" : string.Empty,
                    OfferAmount = offer.OfferAmount,
                    DateRegistration = offer.DateRegistration,
                    Status = offer.Status
                });
            }

            return result.OrderByDescending(o => o.DateRegistration).ToList();
        }

        public async Task AcceptOfferAsync(string offerId)
        {
            var offer = await _offerRepository.GetByIdAsync(offerId);
            if (offer == null) throw new Exception("Oferta no encontrada");

            offer.Status = OfferStatus.Accepted.ToString();
            _offerRepository.Update(offer);

            var property = await _propertyRepository.GetByIdAsync(offer.PropertyId);
            if (property != null)
            {
                property.Status = PropertyStatus.Sold.ToString();
                _propertyRepository.Update(property);
            }

            await _offerRepository.SaveChangesAsync();
        }

        public async Task RejectOfferAsync(string offerId)
        {
            var offer = await _offerRepository.GetByIdAsync(offerId);
            if (offer == null) throw new Exception("Oferta no encontrada");

            offer.Status = OfferStatus.Rejected.ToString();
            _offerRepository.Update(offer);
            await _offerRepository.SaveChangesAsync();
        }
    }
}