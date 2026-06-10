using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public interface IVariantService
{
    public Task<List<Variant>> GetAllAsync();
    public Task<List<Variant>> GetAllByProdutIdAsync(Guid productId);
    public Task<Variant?> GetByIdAsync(Guid id);
    public Task<Variant> CreateAsync(CreateVariantDto createVariantDto);
    public Task<Variant?> UpdateAsync(Guid id, UpdateVariantDto updateVariantDto);
    public Task<bool> DeleteAsync(Guid id);
}
