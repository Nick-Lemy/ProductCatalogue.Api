namespace ProductCatalogue.Api.DTOs;

public class VariantReponseDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Name { get; set; } = null!;
    public string VariantCode { get; set; } = null!;
    public string Colour { get; set; } = null!;
    public string Size { get; set; } = null!;
    public string Material { get; set; } = null!;
    public string? Barcode { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

}