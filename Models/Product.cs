namespace ProductCatalogue.Api.Models;

public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ProductCode { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string TargetMarket { get; set; } = string.Empty;
    public string Season { get; set; } = string.Empty;

    public ProductStatus Status { get; set; } = ProductStatus.DRAFT;
    public ProductReadiness Readiness { get; set; } = ProductReadiness.NOT_READY;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

}
