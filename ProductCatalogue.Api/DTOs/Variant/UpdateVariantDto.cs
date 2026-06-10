using System.ComponentModel.DataAnnotations;

namespace ProductCatalogue.Api.DTOs;

public class UpdateVariantDto
{
    [Required]
    public Guid? ProductId { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string? VariantCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string? Colour { get; set; } = string.Empty;

    [Required]
    [MaxLength(3)]
    public string? Size { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Material { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Barcode { get; set; }
}