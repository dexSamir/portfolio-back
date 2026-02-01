using Microsoft.AspNetCore.Http;
namespace Portfolio.Common.Extensions;
public static class FileExtensions
{
    public static bool IsValidType(this IFormFile file, string type)
        => file.ContentType.StartsWith(type);

    public static bool IsValidSize(this IFormFile file, int mb)
        => file.Length <= mb * 1024 * 1024;

    public static async Task<string> UploadAsync(this IFormFile file, params string[] paths)
    {
        string path = Path.Combine(paths);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        
        
        string fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
        string fullPath = Path.Combine(path, fileName);

        await using var stream = File.Create(fullPath);
        await file.CopyToAsync(stream);
        
        return fileName; 
    }
    
    public static bool DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            return true;
        }
        return false;
    }
}