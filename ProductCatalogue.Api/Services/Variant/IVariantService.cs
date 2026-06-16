using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public interface IVariantService
{
    public Task<List<VariantResponseDto>> GetAllAsync(VariantQueryDto query);
    public Task<VariantResponseDto> GetByIdAsync(Guid id);
    public Task<VariantResponseDto> CreateAsync(CreateVariantDto createVariantDto);
    public Task<VariantResponseDto> UpdateAsync(Guid id, UpdateVariantDto updateVariantDto);
    public Task DeleteAsync(Guid id);
}
