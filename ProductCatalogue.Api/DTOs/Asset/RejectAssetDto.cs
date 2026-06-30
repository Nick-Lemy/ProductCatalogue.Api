using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.DTOs;

public class RejectAssetDto
{
    public required string RejectionReason { get; set; }
}