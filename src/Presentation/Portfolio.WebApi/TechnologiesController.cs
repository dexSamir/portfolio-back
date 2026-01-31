using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Abstraction.Services;
using Portfolio.Application.Dtos.Technology;
using Portfolio.Domain.Enums;

namespace Portfolio.WebAPI;

[Route("api/[controller]/[action]")]
[ApiController]
public class TechnologiesController(ITechnologyService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        return Ok(await service.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TechnologyCreateDto dto)
    {
        return Ok(await service.CreateAsync(dto));
    }

    [HttpPost]
    public async Task<IActionResult> CreateRange( IEnumerable<TechnologyCreateDto> dtos)
    {
        return Ok(await service.CreateBulkAsync(dtos)); 
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(Guid id,  [FromBody] TechnologyUpdateDto dto)
    {
        return Ok(await service.UpdateAsync(id, dto)); 
    }

    [HttpDelete("{dType}")]
    public async Task<IActionResult> Delete([FromQuery] Guid[] ids, EDeleteType dType)
    {
        return Ok(await service.DeleteAsync(ids, dType));
    }
}