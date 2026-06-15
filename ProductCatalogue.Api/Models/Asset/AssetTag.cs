namespace ProductCatalogue.Api.Models;

public class AssetTag
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<Asset> Assets { get; set; } = [];
}