using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public interface IAssetService
{
    public Task<Result<List<AssetResponseDto>>> GetAllAsync(AssetQueryDto query);
    public Task<Result<AssetResponseDto>> GetByIdAsync(Guid id);
    public Task<Result<AssetResponseDto>> CreateAsync(UploadAssetDto uploadAssetDto);
    public Task<Result<AssetResponseDto>> UpdateAsync(Guid id, UpdateAssetDto updateAssetDto);
    public Task<Result> RejectAssetAsync(Guid id, RejectAssetDto rejectAssetDto);
    public Task<Result> ApproveAssetAsync(Guid id);
    public Task<Result> DeleteAsync(Guid id);
}