using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Services.PhotoService;

public interface IPhotoService
{
    public Task<ImageUploadResult> AddPhotoAsync(IFormFile File);
    public Task<DeletionResult> DeletePhotoAsync(string publicId);
}