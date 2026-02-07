using Microsoft.AspNetCore.Http;

namespace Portfolio.Application.Abstraction.Infrastructure;

public interface ICloudinaryService
{
    Task<string> UploadImageAsync(IFormFile file, string folder);
    Task DeleteImageAsync(string imageUrl);
}