using System.ComponentModel.DataAnnotations;

namespace ProductCatalogue.Api.DTOs;

public class VariantQueryDto
{
    public Guid? ProductId { get; set; }
    public string? Name { get; set; }
    public string? VariantCode { get; set; }
    public string? Colour { get; set; }
    public string? Size { get; set; }
    public string? Material { get; set; }
    public string? Barcode { get; set; }
}