using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Properties;
using Application.ViewModels.Properties;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IGenericRepository<Property> _genericRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly  IWebHostEnvironment _env;


        public PropertyService(IGenericRepository<Property> genericRepository,
                                IPropertyRepository propertyRepository,
                                UserManager<AppUser> userManager,
                                IMapper mapper)
        {
            _genericRepository = genericRepository;
            _propertyRepository = propertyRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<PropertyViewModel>> GetAllAsync()
        {
            var properties = await _propertyRepository.GetAllWithDetailsAsync();
            return _mapper.Map<List<PropertyViewModel>>(properties);
            
        }
        
        public async Task<List<PropertyViewModel>> GetByAgentAsync(string agentId){
            var properties = await _propertyRepository.GetByAgentDetailsAsync(agentId);

            return _mapper.Map<List<PropertyViewModel>>(properties);
        }

        public async Task<PropertyViewModel> GetByIdAsync(string id){
            var property= await _propertyRepository.GetByIdWithDetailsAsync(id);
            var vm = _mapper.Map<PropertyViewModel>(property);

            var agent = await _userManager.FindByIdAsync(property.AgentId);

            if(agent != null)
            {
                vm.AgentId = agent.Id;
                vm.AgentName = $"{agent.FirstName} {agent.LastName}";
                vm.AgentPhone = agent.PhoneNumber!;
            }

            return vm;
        }

        public async Task CreateAsync(SavePropertyViewModel vm){
            var propety = _mapper.Map<Property>(vm);

            propety.PropertyCode = Guid.NewGuid().ToString().Substring(0, 6);
            propety.Status = PropertyStatus.Available.ToString();
            propety.RegistrationDate = DateTime.Now;

            foreach(var improvementId in vm.ImprovementIds)
            {
                propety.Improvements.Add(new PropertyImprovements
                {
                   ImprovementId = improvementId,
                    PropertyId = propety.Id 
                });
            }

            foreach(var image in vm.Images)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                var folderPath = Path.Combine("wwwroot", "uploads", "properties");
                
                Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(_env.WebRootPath,folderPath, fileName);

                using(var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                propety.Images.Add(new PropertyImage { Path = $"/uploads/properties/{fileName}", PropertyId = propety.Id});
            }

            await _genericRepository.AddAsync(propety);
        }

        public async Task UpdateAsync(string id, SavePropertyViewModel vm){
            var propety = await _genericRepository.GetByIdAsync(id);

            if(propety == null) throw new Exception("Propiedad no encontrada");

            propety.Description = vm.Description;
            propety.Location = vm.Location;
            propety.Name = vm.Name;
            propety.Price = vm.Price;
            propety.NumberRooms = vm.Rooms;
            propety.NumberBaths = vm.Bathrooms;
            propety.Meters = (int) vm.Meters;

            // Improvements: borrar los viejos y agregar los nuevos
            propety.Improvements.Clear(); // funciona porque EF Core trackea los cambios en la coleccion y borra los registros huérfanos automáticamente (siempre que Cascade Delete esté configurado).
            foreach (var improvementId in vm.ImprovementIds)
            {
                propety.Improvements.Add(new PropertyImprovements
                {
                    ImprovementId = improvementId,
                    PropertyId = propety.Id
                });
            }

            // Imágenes: solo agregar si vienen nuevas (las viejas se conservan)
            if (vm.Images != null && vm.Images.Any())
            {
                foreach (var image in vm.Images)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                    var folderPath = Path.Combine(_env.WebRootPath, "uploads", "properties");
                    Directory.CreateDirectory(folderPath);

                    var filePath = Path.Combine(folderPath, fileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await image.CopyToAsync(stream);

                    propety.Images.Add(new PropertyImage
                    {
                        Path = $"/uploads/properties/{fileName}",
                        PropertyId = propety.Id
                    });
                }
            }


            _genericRepository.Update(propety);
        }
        public async Task DeleteAsync(string id){
            var propety = await _propertyRepository.GetByIdWithDetailsAsync(id);

            foreach(var image in propety.Images)
            {
                var filePath = Path.Combine(_env.WebRootPath, image.Path.TrimStart('/').Replace('/',Path.DirectorySeparatorChar));
                if(File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            _genericRepository.Remove(propety);
            await _genericRepository.SaveChangesAsync();
        }

        public async Task<List<PropertyViewModel>> FilterAsync(PropertyFilterViewModel filters)
        {
            var properties = await _propertyRepository.FilterAsync(
                filters.TypeSaleName,
                filters.PropertyTypeId,
                filters.MinPrice,
                filters.MaxPrice,
                filters.Rooms,
                filters.Bathrooms
            );

            return _mapper.Map<List<PropertyViewModel>>(properties);
        }
    }
}