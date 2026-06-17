using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public interface IAssetService
{
    public Task<List<AssetResponseDto>> GetAllAsync(AssetQueryDto query);
    public Task<AssetResponseDto> GetByIdAsync(Guid id);
    public Task<AssetResponseDto> CreateAsync(UploadAssetDto uploadAssetDto);
    public Task<AssetResponseDto> UpdateAsync(Guid id, UpdateAssetDto updateAssetDto);
    public Task RejectAssetAsync(Guid id, RejectAssetDto rejectAssetDto);
    public Task ApproveAssetAsync(Guid id, ApproveAssetDto approveAssetDto);
    public Task DeleteAsync(Guid id);
}