using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Services.PhotoService;

public class PhotoService : IPhotoService, IScopedDependency
{
    private readonly Cloudinary _cloudinary;

    public PhotoService(IOptionsSnapshot<SiteSettings> settings)
    {
        var acc = new Account
        (
            settings.Value.CloudinarySettings.CloudName,
            settings.Value.CloudinarySettings.ApiKey,
            settings.Value.CloudinarySettings.ApiSecret
        );
        _cloudinary = new Cloudinary(acc);
    }

    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile File)
    {
        var uploadResult = new ImageUploadResult();
        if (File.Length > 0)
        {
            using var stream = File.OpenReadStream();
            var UploadParams = new ImageUploadParams
            {
                File = new FileDescription(File.FileName, stream)
            };
            uploadResult = await _cloudinary.UploadAsync(UploadParams);
        }

        return uploadResult;
    }
    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);
        return result;
    }
}