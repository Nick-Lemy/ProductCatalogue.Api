using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalogue.Api.Models;

public class AssetStatusLog
{
    public Guid Id { get; set; }


    public AssetStatus Status { get; set; }
    public DateTimeOffset ChangedAt { get; set; }

    public string RejectionReason { get; set; } = null;

    [ForeignKey("Asset")]
    public Guid AssetId { get; set; }
    public Asset Asset { get; set; }
}