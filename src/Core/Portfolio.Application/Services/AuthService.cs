using Microsoft.AspNetCore.Identity;
using Portfolio.Application.Abstraction.Services;
using Portfolio.Domain.Entities;

namespace Portfolio.Application.Services;

public class AuthService(
    UserManager<User> userManager,
    SignInManager<User> signInManager)
    : IAuthService
{
    public async Task<bool> LoginAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
            return false;

        var result = await signInManager.PasswordSignInAsync(
            user,
            password,
            isPersistent: false,
            lockoutOnFailure: false);

        return result.Succeeded;
    }
}