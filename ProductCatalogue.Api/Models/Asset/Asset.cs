using System.ComponentModel.DataAnnotations;

namespace ProductCatalogue.Api.Models;

public class Asset
{
    [Key]
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid? VariantId { get; set; }

    public string FileName { get; set; }
    public string FileUrl { get; set; }

    public AssetType AssetType { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public List<AssetTag> Tags { get; set; } = [];

    public AssetStatus Status { get; set; }
    public List<AssetStatusLog> StatusHisoty { get; set; }
    public string RejectionReason { get; set; }
    public DateTimeOffset Uploaded { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

}