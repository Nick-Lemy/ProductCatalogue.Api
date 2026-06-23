using Microsoft.EntityFrameworkCore;
using ProductCatalogue.Api.Data;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Exceptions;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public class AssetTagService(AppDbContext context, ILogger<AssetTagService> logger) : IAssetTagService
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<AssetTagService> _logger = logger;
    public async Task<AssetTag> GetOrCreateByNameAsync(string name)
    {
    AssetTag? tag = await _context.AssetTags
        .FirstOrDefaultAsync(t => EF.Functions.ILike(t.Name, name));

    if (tag is null)
    {
        tag = new AssetTag { Id = Guid.NewGuid(), Name = name };
        _context.AssetTags.Add(tag);
        await _context.SaveChangesAsync();
        _logger.LogInformation("[AssetTag] Created new tag {Name}", name);
    }

    return tag;
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        _logger.LogInformation("[AssetTag] Deleting asset tag with id {Id}", id);
        AssetTag? assetTag = await _context.AssetTags.FirstOrDefaultAsync(t => t.Id == id);

        if(assetTag is null)
            throw new NotFoundException($"Asset tag with id {id} not found");
    
        _context.AssetTags.Remove(assetTag);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByNameAsync(string name)
    {
        _logger.LogInformation("[AssetTag] Deleting asset tag with name {Name}", name);
        AssetTag? assetTag = await _context.AssetTags.FirstOrDefaultAsync(t => t.Name == name);
    
        if(assetTag is null)
            throw new NotFoundException($"Asset tag with name {name} not found");
    
        _context.AssetTags.Remove(assetTag);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<AssetTag>> GetAllAsync()
    {
        _logger.LogInformation("[AssetTag] Fetching all asset tags");
        List<AssetTag> assetTags = await _context.AssetTags.AsNoTracking().ToListAsync();
        _logger.LogInformation("[AssetTag] Fetched {Count} asset tags", assetTags.Count);
        return assetTags;
    }

    public async Task<AssetTag> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("[AssetTag] Fetching asset tag with id {Id}", id);
        AssetTag? assetTag = await _context.AssetTags.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);

        if(assetTag is null)
            throw new NotFoundException($"Asset tag with id {id} not found");
        _logger.LogInformation("[AssetTag] Fetched asset tag with id {Id}", id);
        return assetTag;
    }

}