using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.DTOs;

public class AssetStatusLogResponseDto
{
    public Guid Id { get; init; }
    public AssetStatus Status { get; init; }
    public DateTimeOffset ChangedAt { get; init; }
    public string? RejectionReason { get; init; }
}
