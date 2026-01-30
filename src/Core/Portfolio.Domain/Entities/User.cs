using Microsoft.AspNetCore.Identity;
namespace Portfolio.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string FullName { get; set; }
    public string? ProfileImageUrl { get; set; }

    // Auth / Tokens
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

}