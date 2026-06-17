using System.ComponentModel.DataAnnotations;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public class CloudinaryService(Cloudinary cloudinary): IFileStorageService
{
    private readonly Cloudinary _cloudinary = cloudinary;
    private static readonly string[] AllowedExtensions =
    [".jpg", ".jpeg", ".png", ".webp", ".mp4", ".mov", ".pdf", ".docx"];

    private const long MaxFileSizeBytes = 10 * 1024 * 1024;

    public async Task<StorageUploadResult> UploadFileAsync(IFormFile file, string folder)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            throw new ValidationException($"File extension {extension} is not allowed. Allowed types: {string.Join(", ", AllowedExtensions)}");

        if (file.Length > MaxFileSizeBytes)
            throw new ValidationException(
                $"File size {file.Length / 1024 / 1024}MB exceeds the maximum allowed size of {MaxFileSizeBytes / 1024 / 1024}MB");
    
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