using System.ComponentModel.DataAnnotations;

namespace ProductCatalogue.Api.DTOs;

public class VariantQueryDto
{
    public Guid? ProductId { get; set; }
    public string? Name { get; set; } = string.Empty;
    public string? VariantCode { get; set; } = string.Empty;
    public string? Colour { get; set; } = string.Empty;
    public string? Size { get; set; } = string.Empty;
    public string? Material { get; set; } = string.Empty;
    public string? Barcode { get; set; }
}