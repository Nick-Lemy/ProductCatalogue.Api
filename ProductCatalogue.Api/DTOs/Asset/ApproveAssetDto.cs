using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.DTOs;
public class ApproveAssetDto
{
    public required AssetStatus NewStatus { get; set; }
}