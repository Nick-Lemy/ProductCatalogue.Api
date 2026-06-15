
using ProductCatalogue.Api.Models;
namespace ProductCatalogue.Api.DTOs;

public class ProductResponseDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string ProductCode { get; init; } = string.Empty;
    public string Brand { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public string TargetMarket { get; init; } = string.Empty;

    public string Season { get; init; } = string.Empty;
    public ProductStatus Status { get; init; }
    public ProductReadiness Readiness { get; init; }

    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }

}
