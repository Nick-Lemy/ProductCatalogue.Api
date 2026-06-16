using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProductCatalogue.Api.Models;

[Index(nameof(VariantCode), IsUnique = true)]
[Index(nameof(Barcode), IsUnique = true)]
public class Variant
{
    [Key]
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }

    [MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(50)]
    public required string VariantCode { get; set; }

    [MaxLength(50)]
    public required string Colour { get; set; }

    [MaxLength(4)]
    public required string Size { get; set; }

    [MaxLength(100)]
    public required string Material { get; set; }

    [MaxLength(100)]
    public string? Barcode { get; set; }

    public ICollection<Asset> Assets { get; set; } = [];

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

}