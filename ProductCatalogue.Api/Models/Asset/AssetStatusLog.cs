using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalogue.Api.Models;

public class AssetStatusLog
{
    [Key]
    public Guid Id { get; set; }
    public required AssetStatus Status { get; set; }

    public DateTimeOffset ChangedAt { get; set; } = DateTimeOffset.UtcNow;

    public string? RejectionReason { get; set; }

    public required Guid AssetId { get; set; }

    [ForeignKey(nameof(AssetId))]
    public Asset? Asset { get; set; }
}