using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public interface IAssetTagService
{
    Task<ICollection<AssetTag>> GetAllAsync();
    Task<AssetTag> GetOrCreateByNameAsync(string name);
    Task<AssetTag> GetByIdAsync(Guid id);
    Task DeleteByIdAsync(Guid id);
    Task DeleteByNameAsync(string name);
}