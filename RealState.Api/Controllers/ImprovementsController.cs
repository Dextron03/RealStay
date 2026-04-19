using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Administrator.Improvements;
using Domain.Interfaces;
using Domain.Entities;
using AutoMapper;

namespace RealStay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImprovementsController : ControllerBase
    {
        private readonly IGenericRepository<PropertyImprovement> _improvementRepository;
        private readonly IMapper _mapper;

        public ImprovementsController(IGenericRepository<PropertyImprovement> improvementRepository, IMapper mapper)
        {
            _improvementRepository = improvementRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Developer")]
        public async Task<ActionResult<List<ImprovementsDto>>> List()
        {
            var improvements = await _improvementRepository.GetAllAsync();
            if (improvements == null || improvements.Count == 0)
                return NoContent();

            return Ok(_mapper.Map<List<ImprovementsDto>>(improvements));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator, Developer")]
        public async Task<ActionResult<ImprovementsDto>> GetById(string id)
        {
            var improvement = await _improvementRepository.GetByIdAsync(id);
            if (improvement == null)
                return NotFound();

            return Ok(_mapper.Map<ImprovementsDto>(improvement));
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<PropertyImprovement>> Create([FromBody] CreateImprovementDto dto)
        {
            var improvement = _mapper.Map<PropertyImprovement>(dto);
            await _improvementRepository.AddAsync(improvement);
            await _improvementRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = improvement.Id }, improvement);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(string id, [FromBody] EditImprovementDto dto)
        {
            var improvement = await _improvementRepository.GetByIdAsync(id);
            if (improvement == null)
                return NotFound();

            _mapper.Map(dto, improvement);
            _improvementRepository.Update(improvement);
            await _improvementRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(string id)
        {
            var improvement = await _improvementRepository.GetByIdAsync(id);
            if (improvement == null)
                return NotFound();

            _improvementRepository.Remove(improvement);
            await _improvementRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}