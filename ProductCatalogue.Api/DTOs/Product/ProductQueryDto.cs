namespace ProductCatalogue.Api.DTOs;

public class ProductQueryDto
{
    public string? Name { get; set; }
    public string? ProductCode { get; set; }
    public string? Brand { get; set; }
    public string? Category { get; set; }
    public string? Status { get; set; }
    public string? Readiness { get; set; }
}