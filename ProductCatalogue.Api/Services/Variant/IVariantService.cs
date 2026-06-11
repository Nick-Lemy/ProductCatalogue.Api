using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public interface IVariantService
{
    public Task<List<VariantReponseDto>> GetAllAsync(VariantQueryDto query);
    public Task<VariantReponseDto> GetByIdAsync(Guid id);
    public Task<VariantReponseDto> CreateAsync(CreateVariantDto createVariantDto);
    public Task<VariantReponseDto> UpdateAsync(Guid id, UpdateVariantDto updateVariantDto);
    public Task DeleteAsync(Guid id);
}
