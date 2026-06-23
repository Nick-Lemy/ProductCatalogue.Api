using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.DTOs;

public class AssetResponseDto
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public Guid? VariantId { get; init; }
    public required string FileName { get; init; }
    public required string FileUrl { get; init; }
    public required string FilePublicId { get; init; }
    public required AssetType AssetType { get; init; }

    public required string Title { get; init; }
    public required string Description { get; init; }

    public string? RejectionReason { get; init; }
    public AssetStatus Status { get; init; }
    public List<AssetStatusLogResponseDto> StatusHistory { get; init; } = [];
    public List<AssetTagResponseDto> Tags { get; init; } = [];

    public DateTimeOffset UploadedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }

}