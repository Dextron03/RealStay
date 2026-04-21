using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Administrator.Sales;
using Domain.Interfaces;
using Domain.Entities;
using AutoMapper;

namespace RealStay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleTypesController : ControllerBase
    {
        private readonly IGenericRepository<TypeSale> _saleTypeRepository;
        private readonly IMapper _mapper;

        public SaleTypesController(IGenericRepository<TypeSale> saleTypeRepository, IMapper mapper)
        {
            _saleTypeRepository = saleTypeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Developer")]
        public async Task<ActionResult<List<SalesTypeDto>>> List()
        {
            var saleTypes = await _saleTypeRepository.GetAllAsync();
            if (saleTypes == null || saleTypes.Count == 0)
                return NoContent();

            return Ok(_mapper.Map<List<SalesTypeDto>>(saleTypes));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator, Developer")]
        public async Task<ActionResult<SalesTypeDto>> GetById(string id)
        {
            var saleType = await _saleTypeRepository.GetByIdAsync(id);
            if (saleType == null)
                return NotFound();

            return Ok(_mapper.Map<SalesTypeDto>(saleType));
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<TypeSale>> Create([FromBody] CreateTypeSaleDto dto)
        {
            var saleType = _mapper.Map<TypeSale>(dto);
            await _saleTypeRepository.AddAsync(saleType);
            await _saleTypeRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = saleType.Id }, saleType);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(string id, [FromBody] EditTypeSaleDto dto)
        {
            var saleType = await _saleTypeRepository.GetByIdAsync(id);
            if (saleType == null)
                return NotFound();

            _mapper.Map(dto, saleType);
            _saleTypeRepository.Update(saleType);
            await _saleTypeRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(string id)
        {
            var saleType = await _saleTypeRepository.GetByIdAsync(id);
            if (saleType == null)
                return NotFound();

            _saleTypeRepository.Remove(saleType);
            await _saleTypeRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}