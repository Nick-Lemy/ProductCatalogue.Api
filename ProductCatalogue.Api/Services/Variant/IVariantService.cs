using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public interface IVariantService
{
    public Task<Result<List<VariantResponseDto>>> GetAllAsync(VariantQueryDto query);
    public Task<Result<VariantResponseDto>> GetByIdAsync(Guid id);
    public Task<Result<VariantResponseDto>> CreateAsync(CreateVariantDto createVariantDto);
    public Task<Result<VariantResponseDto>> UpdateAsync(Guid id, UpdateVariantDto updateVariantDto);
    public Task<Result> DeleteAsync(Guid id);
}
