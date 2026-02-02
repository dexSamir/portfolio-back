using Microsoft.AspNetCore.Http;
using Portfolio.Application.Abstraction.Infrastructure;
using Portfolio.Application.Exceptions.Image;
using Portfolio.Common.Extensions;

namespace Portfolio.Infrastructure.ExternalServices;

public class FileService : IFileService
{
    public Task DeleteImageIfNotDefault(string imageUrl, string folder)
    {
        string defaultImage = $"/{folder}/default.jpg";
        if (!string.IsNullOrEmpty(imageUrl) && imageUrl != defaultImage)
        {
            string fileName = Path.Combine("wwwroot", folder, imageUrl);
            FileExtensions.DeleteFile(fileName); 
        }

        return Task.CompletedTask;
    }

    public async Task<string?> ProcessImageAsync(IFormFile? file, string directory, string fileType, int maxSize, string? existingFilePath = null)
    {
        if (file == null)
            return existingFilePath;

        if (!file.IsValidType(fileType))
            throw new UnsupportedFileTypeException($"File must be of type image!");

        if (!file.IsValidSize(maxSize))
            throw new UnsupportedFileSizeException($"File size must be less than {maxSize}MB!");

        if(!string.IsNullOrEmpty(existingFilePath))
            FileExtensions.DeleteFile(Path.Combine("wwwroot", directory, existingFilePath));

        return await file.UploadAsync("wwwroot", directory);
    }
}