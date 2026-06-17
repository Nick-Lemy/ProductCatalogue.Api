using Mapster;
using Microsoft.EntityFrameworkCore;
using ProductCatalogue.Api.Data;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Exceptions;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public class AssetService(
    AppDbContext context,
    IFileStorageService fileStorageService,
     IAssetTagService assetTagService,
    ILogger<AssetService> logger) : IAssetService
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<AssetService> _logger = logger;
    private readonly IFileStorageService _fileStorageService = fileStorageService;
    private readonly IAssetTagService _assetTagService = assetTagService;
    public async Task<AssetResponseDto> CreateAsync(UploadAssetDto uploadAssetDto)
    {
        _logger.LogInformation("[Asset] Creating asset for product {ProductId}", uploadAssetDto.ProductId);

        bool productExists = await _context.Products.AnyAsync(p => p.Id == uploadAssetDto.ProductId);
        if (!productExists)
            throw new NotFoundException($"Product with id {uploadAssetDto.ProductId} not found");

        StorageUploadResult uploadResult = await _fileStorageService.UploadFileAsync(uploadAssetDto.File, "assets");

        Asset asset = uploadAssetDto.Adapt<Asset>();
        asset.FilePublicId = uploadResult.PublicId;
        asset.FileUrl = uploadResult.Url;

        if (uploadAssetDto.TagNames?.Count > 0)
        {
            foreach (var name in uploadAssetDto.TagNames)
            {
                AssetTag tag = await _assetTagService.GetOrCreateByNameAsync(name);
                asset.Tags.Add(tag);
            }
        }

        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();

        _logger.LogInformation("[Asset] Asset created successfully for product {ProductId}", uploadAssetDto.ProductId);
        return asset.Adapt<AssetResponseDto>();
    }

    public async Task<List<AssetResponseDto>> GetAllAsync(AssetQueryDto query)
    {
        _logger.LogInformation("[Asset] Fetching assets with query {@Query}", query);
        List<Asset> assets = await _context.Assets.Include(a => a.Tags).AsNoTracking()
            .Where(a => query.ProductId == null || a.ProductId == query.ProductId)
            .Where(a => query.VariantId == null || a.VariantId == query.VariantId)
            .Where(a => query.FileName == null || a.FileName == query.FileName)
            .Where(a => query.Title == null || a.Title == query.Title)
            .Where(a => query.Description == null || a.Description == query.Description)
            .Where(a => query.AssetType == null || a.AssetType == query.AssetType)
            .Where(a => query.RejectionReason == null || a.RejectionReason == query.RejectionReason)
            .Where(a => query.Status == null || a.Status == query.Status)
            .Where(a => query.TagName == null || a.Tags.Any(t => t.Name.Equals(query.TagName, StringComparison.CurrentCultureIgnoreCase)))
            .ToListAsync();

        _logger.LogInformation("[Asset] Fetched {Count} assets", assets.Count);
        return assets.Adapt<List<AssetResponseDto>>();
    }

    public async Task<AssetResponseDto> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("[Asset] Fetching asset with id {Id}", id);
        Asset? asset = await _context.Assets.Include(a => a.Tags).AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);

        if(asset is null)
            throw new NotFoundException($"Asset with id {id} not found");

        _logger.LogInformation("[Asset] Asset fetched successfully with id {Id}", id);
        return asset.Adapt<AssetResponseDto>();
    }

    public async Task<AssetResponseDto> UpdateAsync(Guid id, UpdateAssetDto updateAssetDto)
    {
        throw new NotImplementedException();
    }
    public async Task DeleteAsync(Guid id)
    {
        Asset? asset = await _context.Assets.FirstOrDefaultAsync(a => a.Id == id);
        if(asset is null)
            throw new NotFoundException($"Asset with id {id} not found");
        _context.Assets.Remove(asset);
        await _fileStorageService.DeleteFileAsync(asset.FilePublicId);
        await _context.SaveChangesAsync();
    }

    public async Task RejectAssetAsync(Guid id, RejectAssetDto rejectAssetDto)
    {
        _logger.LogInformation("[Asset] Rejecting asset with id {Id} for reason: {Reason}", id, rejectAssetDto.RejectionReason);
        Asset? asset = await _context.Assets.FirstOrDefaultAsync(a => a.Id == id);
        if(asset is null)
            throw new NotFoundException($"Asset with id {id} not found");

        asset.Status = AssetStatus.REJECTED;
        asset.RejectionReason = rejectAssetDto.RejectionReason;

        AssetStatusLog log = new ()
        {
            AssetId = asset.Id,
            ChangedAt = DateTime.UtcNow,
            Status = asset.Status,
            RejectionReason = rejectAssetDto.RejectionReason
        };
        asset.StatusHistory.Add(log);
        await _context.SaveChangesAsync();
        _logger.LogInformation("[Asset] Asset with id {Id} rejected successfully", id);
    }

    public async Task ApproveAssetAsync(Guid id, ApproveAssetDto approveAssetDto)
    {
        _logger.LogInformation("[Asset] Approving asset with id {Id}", id);
        Asset? asset = await _context.Assets.FirstOrDefaultAsync(a => a.Id == id);
        if(asset is null)
            throw new NotFoundException($"Asset with id {id} not found");

        asset.Status = approveAssetDto.NewStatus;
        AssetStatusLog log = new ()
        {
            AssetId = asset.Id,
            ChangedAt = DateTime.UtcNow,
            Status = asset.Status
        };
        asset.StatusHistory.Add(log);
        await _context.SaveChangesAsync();
        _logger.LogInformation("[Asset] Asset with id {Id} approved successfully", id);
    }
}