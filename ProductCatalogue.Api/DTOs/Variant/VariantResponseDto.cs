namespace ProductCatalogue.Api.DTOs;

public class VariantReponseDto
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public string Name { get; init; } = null!;
    public string VariantCode { get; init; } = null!;
    public string Colour { get; init; } = null!;
    public string Size { get; init; } = null!;
    public string Material { get; init; } = null!;
    public string? Barcode { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }

}