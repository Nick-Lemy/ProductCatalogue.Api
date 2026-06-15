
using ProductCatalogue.Api.Models;
namespace ProductCatalogue.Api.DTOs;

public class ProductResponseDto
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string ProductCode { get; private set; } = string.Empty;
    public string Brand { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public string TargetMarket { get; private set; } = string.Empty;

    public string Season { get; private set; } = string.Empty;
    public ProductStatus Status { get; private set; }
    public ProductReadiness Readiness { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

}
