using System.ComponentModel.DataAnnotations;

namespace ProductCatalogue.Api.DTOs;

public class UpdateVariantDto
{
    public Guid? ProductId { get; set; }

    [MaxLength(100)]
    public string? Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? VariantCode { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Colour { get; set; } = string.Empty;

    [MaxLength(3)]
    public string? Size { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Material { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Barcode { get; set; }
}