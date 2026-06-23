using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.DTOs;

public class UpdateAssetDto
{
    public Guid? ProductId { get; set; }
    public Guid? VariantId { get; set; }
    public IFormFile? File { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public AssetType? AssetType { get; set; }
}