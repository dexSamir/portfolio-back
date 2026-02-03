using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Abstraction.Services;
using Portfolio.Application.Dtos.Testimonial;
using Portfolio.Domain.Enums;

namespace Portfolio.WebAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class TestimonialsController(ITestimonialService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetApproved()
        => Ok(await service.GetApprovedAsync());

    [HttpGet("{status}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetByStatus(ETestimonialStatus status)
        => Ok(await service.GetByStatusAsync(status));

    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> GetAll()
        => Ok(await service.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] TestimonialCreateDto dto)
        => Ok(await service.CreateAsync(dto));

    [HttpPatch("{id}/approve")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Approve(Guid id)
        => Ok(await service.ChangeStatusAsync(id, ETestimonialStatus.Approved));

    [HttpPatch("{id}/deny")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Deny(Guid id)
        => Ok(await service.ChangeStatusAsync(id, ETestimonialStatus.Denied));

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(Guid id)
        => Ok(await service.DeleteAsync(id));
}