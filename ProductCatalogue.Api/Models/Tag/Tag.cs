namespace ProductCatalogue.Api.Models;

public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<Asset> Assets { get; set; } = [];
}