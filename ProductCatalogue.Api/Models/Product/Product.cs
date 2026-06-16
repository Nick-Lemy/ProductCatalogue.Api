using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ProductCatalogue.Api.Models;

[Index(nameof(ProductCode), IsUnique = true)]
public class Product
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(500)]
    public required string Description { get; set; }

    [MaxLength(50)]
    public required string ProductCode { get; set; }

    [MaxLength(100)]
    public required string Brand { get; set; }

    [MaxLength(100)]
    public required string Category { get; set; }

    [MaxLength(100)]
    public required string TargetMarket { get; set; }

    [MaxLength(100)]
    public required string Season { get; set; }

    public ICollection<Variant> Variants { get; set; } = [];
    public ICollection<Asset> Assets { get; set; } = [];
    public ProductStatus Status { get; set; } = ProductStatus.DRAFT;
    public ProductReadiness Readiness { get; set; } = ProductReadiness.NOT_READY;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}
