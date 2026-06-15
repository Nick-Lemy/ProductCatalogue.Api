using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalogue.Api.Models;

public class Variant
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey(nameof(Product))]
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string VariantCode { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Colour { get; set; } = string.Empty;

    [MaxLength(4)]
    public string Size { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Material { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Barcode { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

}