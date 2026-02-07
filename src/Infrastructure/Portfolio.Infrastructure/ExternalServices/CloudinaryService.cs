using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Portfolio.Application.Abstraction.Infrastructure;

namespace Portfolio.Infrastructure.ExternalServices;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration config)
    {
        var acc = new Account(
            config["Cloudinary:CloudName"],
            config["Cloudinary:ApiKey"],
            config["Cloudinary:ApiSecret"]
        );

        _cloudinary = new Cloudinary(acc);
    }

    public async Task<string> UploadImageAsync(IFormFile file, string folder)
    {
        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = folder,
            UseFilename = true,
            UniqueFilename = true,
            Overwrite = false
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error != null)
            throw new Exception(result.Error.Message);

        return result.SecureUrl.ToString();
    }

    public async Task DeleteImageAsync(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl)) return;

        if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            return; 

        var uri = new Uri(imageUrl);

        var publicId = string.Join("/",
            uri.AbsolutePath
                .Split('/')
                .SkipWhile(x => x != "upload")
                .Skip(1)
        );

        publicId = Path.ChangeExtension(publicId, null);

        await _cloudinary.DestroyAsync(
            new DeletionParams(publicId)
        );
    }

}