using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.Abstraction.Services;
using Portfolio.Application.Dtos.Auth;

namespace Portfolio.WebAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var success = await authService.LoginAsync(dto.Email, dto.Password);
        if (!success)
            return Unauthorized();

        return Ok("Login successful");
    }
}
