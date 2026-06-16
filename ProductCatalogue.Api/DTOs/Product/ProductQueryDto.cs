using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.DTOs;

public class ProductQueryDto
{
    public string? Name { get; set; }
    public string? ProductCode { get; set; }
    public string? Brand { get; set; }
    public string? Category { get; set; }
    public ProductStatus? Status { get; set; }
    public ProductReadiness? Readiness { get; set; }
}