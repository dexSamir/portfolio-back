namespace Portfolio.Application.Abstraction.Services;
public interface IAuthService
{
    Task<bool> LoginAsync(string email, string password);
}