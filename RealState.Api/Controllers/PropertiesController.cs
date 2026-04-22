using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Agent;
using Domain.Interfaces;
using AutoMapper;

namespace RealStay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator, Developer")]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public PropertiesController(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AgentPropertyDto>>> List()
        {
            var properties = await _propertyRepository.GetAllWithDetailsAsync();
            if (properties == null || properties.Count == 0)
                return NoContent();

            return Ok(_mapper.Map<List<AgentPropertyDto>>(properties));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AgentPropertyDto>> GetById(string id)
        {
            var property = await _propertyRepository.GetByIdWithDetailsAsync(id);
            if (property == null)
                return NotFound();

            return Ok(_mapper.Map<AgentPropertyDto>(property));
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<AgentPropertyDto>> GetByCode(string code)
        {
            var property = await _propertyRepository.GetByCodeAsync(code);
            if (property == null)
                return NotFound();

            return Ok(_mapper.Map<AgentPropertyDto>(property));
        }
    }
}