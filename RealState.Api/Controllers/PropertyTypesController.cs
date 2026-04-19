using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Administrator.Properties;
using Domain.Interfaces;
using Domain.Entities;
using AutoMapper;

namespace RealStay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyTypesController : ControllerBase
    {
        private readonly IGenericRepository<PropertyType> _propertyTypeRepository;
        private readonly IMapper _mapper;

        public PropertyTypesController(IGenericRepository<PropertyType> propertyTypeRepository, IMapper mapper)
        {
            _propertyTypeRepository = propertyTypeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Developer")]
        public async Task<ActionResult<List<PropertiesDto>>> List()
        {
            var propertyTypes = await _propertyTypeRepository.GetAllAsync();
            if (propertyTypes == null || propertyTypes.Count == 0)
                return NoContent();

            return Ok(_mapper.Map<List<PropertiesDto>>(propertyTypes));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator, Developer")]
        public async Task<ActionResult<PropertiesDto>> GetById(string id)
        {
            var propertyType = await _propertyTypeRepository.GetByIdAsync(id);
            if (propertyType == null)
                return NotFound();

            return Ok(_mapper.Map<PropertiesDto>(propertyType));
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<PropertyType>> Create([FromBody] CreatePropertyDto dto)
        {
            var propertyType = _mapper.Map<PropertyType>(dto);
            await _propertyTypeRepository.AddAsync(propertyType);
            await _propertyTypeRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = propertyType.Id }, propertyType);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(string id, [FromBody] EditPropertyDto dto)
        {
            var propertyType = await _propertyTypeRepository.GetByIdAsync(id);
            if (propertyType == null)
                return NotFound();

            _mapper.Map(dto, propertyType);
            _propertyTypeRepository.Update(propertyType);
            await _propertyTypeRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(string id)
        {
            var propertyType = await _propertyTypeRepository.GetByIdAsync(id);
            if (propertyType == null)
                return NotFound();

            _propertyTypeRepository.Remove(propertyType);
            await _propertyTypeRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}