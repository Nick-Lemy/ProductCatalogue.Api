using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public class CloudinaryService(Cloudinary cloudinary): IFileStorageService
{
    private readonly Cloudinary _cloudinary = cloudinary;


    public async Task<StorageUploadResult> UploadFileAsync(IFormFile file, string folder)
    {
        using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "product-catalogue"
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if(result.Error is not null)
            throw new Exception($"File upload failed: {result.Error.Message}");

        return new StorageUploadResult()
        {
            PublicId = result.PublicId,
            Url = result.SecureUrl.ToString()
        };
    }
    public async Task DeleteFileAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        await _cloudinary.DestroyAsync(deleteParams);
    }
}