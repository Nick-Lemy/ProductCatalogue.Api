using System.ComponentModel.DataAnnotations;

namespace ProductCatalogue.Api.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string TargetMarket { get; set; } = string.Empty;
    public string Season { get; set; } = string.Empty;

    public ProductStatus Status { get; set; }
    public ProductReadiness Readiness { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }


    public enum ProductStatus
    {
        DRAFT, IN_REVIEW, PUBLISHED, ARCHIVED,
    }
    public enum ProductReadiness
    {
        READY, NOT_READY
    }
}
