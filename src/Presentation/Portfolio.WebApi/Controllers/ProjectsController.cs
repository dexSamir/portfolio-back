using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Abstraction.Services;
using Portfolio.Application.Dtos.Project;
using Portfolio.Domain.Enums;

namespace Portfolio.WebAPI.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ProjectsController(IProjectService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await service.GetByIdAsync(id));

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create([FromForm] ProjectCreateDto dto)
        => Ok(await service.CreateAsync(dto));

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> CreateRange([FromBody] IEnumerable<ProjectCreateDto> dtos)
        => Ok(await service.CreateBulkAsync(dtos));

    [HttpPatch("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Update(Guid id, [FromForm] ProjectUpdateDto dto)
        => Ok(await service.UpdateAsync(id, dto));

    [HttpDelete("{dType}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete([FromQuery] Guid[] ids, EDeleteType dType)
    {
        return Ok(await service.DeleteAsync(ids, dType));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Restore([FromQuery] Guid[] ids)
        => Ok(await service.RestoreAsync(ids));

    [HttpPost]
    public async Task<IActionResult> GetByTechnology([FromBody] Guid[] technologyIds)
        => Ok(await service.GetByTechnologyAsync(technologyIds));

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> AddTechnologies([FromQuery] Guid projectId, [FromBody] IEnumerable<Guid> techIds)
        => Ok(await service.AddTechnologiesAsync(projectId, techIds));

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> RemoveTechnologies([FromQuery] Guid projectId, [FromBody] IEnumerable<Guid> techIds)
        => Ok(await service.RemoveTechnologiesAsync(projectId, techIds));
}
