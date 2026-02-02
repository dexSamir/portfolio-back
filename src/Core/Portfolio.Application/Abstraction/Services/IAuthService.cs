namespace Portfolio.Application.Abstraction.Services;
public interface IAuthService
{
    Task<string> LoginAsync(string email, string password); 
}