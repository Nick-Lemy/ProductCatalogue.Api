namespace ProductCatalogue.Api.DTOs;

public class CreateAssetTagDto
{
    public required List<string> Names {get; set; } = [];
    public required Guid AssetId { get; set; }
}