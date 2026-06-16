using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public interface IFileStorageService
{
    public Task<StorageUploadResult> UploadFileAsync(IFormFile file, string folder);
    public Task DeleteFileAsync(string publicId);
}

