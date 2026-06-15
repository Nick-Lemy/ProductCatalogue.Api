namespace ProductCatalogue.Api.DTOs;

public class VariantReponseDto
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public string Name { get; private set; } = null!;
    public string VariantCode { get; private set; } = null!;
    public string Colour { get; private set; } = null!;
    public string Size { get; private set; } = null!;
    public string Material { get; private set; } = null!;
    public string? Barcode { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

}