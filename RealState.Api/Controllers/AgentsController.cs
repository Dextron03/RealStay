using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Administrator.Agent;
using Application.DTOs.Agent;
using Domain.Interfaces;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace RealStay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator, Developer")]
    public class AgentsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public AgentsController(UserManager<AppUser> userManager, IPropertyRepository propertyRepository, IMapper mapper)
        {
            _userManager = userManager;
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AgentListDto>>> List()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var agents = new List<AppUser>();

            foreach (var user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, "Agent"))  
                {
                    agents.Add(user);
                }
            }

            if (agents == null || agents.Count == 0)
                return NoContent();

            return Ok(_mapper.Map<List<AgentListDto>>(agents));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AgentListDto>> GetById(string id)
        {
            var agent = await _userManager.FindByIdAsync(id);
            if (agent == null)
                return NotFound();

            var isAgent = await _userManager.IsInRoleAsync(agent, "Agent");  
            if (!isAgent)
                return NotFound("El usuario no es un agente");

            return Ok(_mapper.Map<AgentListDto>(agent));
        }

        [HttpGet("{id}/properties")]
        public async Task<ActionResult<List<AgentPropertyDto>>> GetAgentProperty(string id)
        {
            var agent = await _userManager.FindByIdAsync(id);
            if (agent == null)
                return NotFound("Agente no encontrado");

            var isAgent = await _userManager.IsInRoleAsync(agent, "Agent");  
            if (!isAgent)
                return NotFound("El usuario no es un agente");

            var properties = await _propertyRepository.GetByAgentDetailsAsync(id);
            if (properties == null || properties.Count == 0)
                return NoContent();

            return Ok(_mapper.Map<List<AgentPropertyDto>>(properties));
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ChangeStatus(string id, [FromQuery] bool active)
        {
            var agent = await _userManager.FindByIdAsync(id);
            if (agent == null)
                return NotFound("Agente no encontrado");

            var isAgent = await _userManager.IsInRoleAsync(agent, "Agent");  
            if (!isAgent)
                return NotFound("El usuario no es un agente");

            agent.IsActive = active;
            var result = await _userManager.UpdateAsync(agent);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return NoContent();
        }
    }
}