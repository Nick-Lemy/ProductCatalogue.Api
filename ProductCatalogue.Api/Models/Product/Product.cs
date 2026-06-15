using System.ComponentModel.DataAnnotations;

namespace ProductCatalogue.Api.Models;

public class Product
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(50)]
    public string ProductCode { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Brand { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Category { get; set; } = string.Empty;

    [MaxLength(100)]
    public string TargetMarket { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Season { get; set; } = string.Empty;

    public ProductStatus Status { get; set; } = ProductStatus.DRAFT;
    public ProductReadiness Readiness { get; set; } = ProductReadiness.NOT_READY;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

}
