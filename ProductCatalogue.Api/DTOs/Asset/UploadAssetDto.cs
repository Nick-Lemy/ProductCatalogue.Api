using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.DTOs;

public class UploadAssetDto
{
    public required Guid ProductId { get; set; }
    public Guid? VariantId { get; set; }
    public required IFormFile File { get; set; }
    public required string FileName { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required AssetType AssetType { get; set; }
    public required ICollection<string>? TagNames { get; set; }
}
