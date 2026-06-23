using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.DTOs;
public class AssetQueryDto
{
    public Guid? ProductId { get; set; }
    public Guid? VariantId { get; set; }
    public string? FileName { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public AssetType? AssetType { get; set; }
    public string? RejectionReason { get; set; }
    public AssetStatus? Status { get; set; }
    public string? TagName { get; set; }
}