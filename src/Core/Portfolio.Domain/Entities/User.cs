using Microsoft.AspNetCore.Identity;
namespace Portfolio.Domain.Entities;


public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }

    // Auth / Tokens
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

}