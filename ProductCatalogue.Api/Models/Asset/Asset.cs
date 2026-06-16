using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalogue.Api.Models;

public class Asset
{
    [Key]
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }
    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }

    public Guid? VariantId { get; set; }
    [ForeignKey(nameof(VariantId))]
    public Variant? Variant { get; set; }

    [MaxLength(255)]
    public required string FileName { get; set; }

    [MaxLength(2048)]
    public required string FileUrl { get; set; }
    public required AssetType AssetType { get; set; }


    [MaxLength(100)]
    public required string Title { get; set; }

    [MaxLength(500)]
    public required string Description { get; set; }

    [MaxLength(500)]
    public string? RejectionReason { get; set; }

    public ICollection<AssetTag> Tags { get; set; } = [];
    public AssetStatus Status { get; set; } = AssetStatus.PENDING_REVIEW;
    public ICollection<AssetStatusLog> StatusHistory { get; set; } = [];

    public DateTimeOffset UploadedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

}