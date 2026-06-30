using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Extensions;
using ProductCatalogue.Api.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductCatalogue.Api.Controllers;

[ApiController]
[Authorize]
[Route("assets")]
public class AssetsController(
    IAssetService assetService
): ControllerBase
{
    private readonly IAssetService _assetService = assetService;

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all assets",
        Description = "Fetches a list of assets based on optional query parameters for filtering.")]
    [SwaggerResponse(200, "List of assets fetched successfully", typeof(List<AssetResponseDto>))]
    public async Task<ActionResult<List<AssetResponseDto>>> GetAll(
        [FromQuery] AssetQueryDto query)
    {
        return (await _assetService.GetAllAsync(query)).ToActionResult();
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get asset by ID",
        Description = "Fetches an asset by its unique identifier.")]
    [SwaggerResponse(200, "Asset fetched successfully", typeof(AssetResponseDto))]
    public async Task<ActionResult<AssetResponseDto>> GetById(Guid id)
    {
        return (await _assetService.GetByIdAsync(id)).ToActionResult();
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create asset",
        Description = "Creates a new asset for a product.")]
    [SwaggerResponse(201, "Asset created successfully", typeof(AssetResponseDto))]
    public async Task<ActionResult<AssetResponseDto>> Create([FromForm] UploadAssetDto uploadAssetDto)
    {
        var result = await _assetService.CreateAsync(uploadAssetDto);

        if(!result.IsSuccess)
            return result.ToActionResult();
    
        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }
    
    [HttpPost("{id}/reject")]
    [SwaggerOperation(
        Summary = "Reject asset",
        Description = "Rejects an existing asset.")]
    [SwaggerResponse(204, "Asset rejected successfully", typeof(void))]
    public async Task<ActionResult> Reject(Guid id, [FromBody] RejectAssetDto rejectAssetDto)
    {
        return (await _assetService.RejectAssetAsync(id, rejectAssetDto)).ToActionResult();
    }

    [HttpPost("{id}/approve")]
    [SwaggerOperation(
        Summary = "Approve asset",
        Description = "Approves an existing asset.")]
    [SwaggerResponse(204, "Asset approved successfully", typeof(void))]
    public async Task<ActionResult> Approve(Guid id)
    {
        return (await _assetService.ApproveAssetAsync(id)).ToActionResult();
    }

    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Update asset",
        Description = "Updates an existing asset.")]
    [SwaggerResponse(204, "Asset updated successfully", typeof(AssetResponseDto))]
    public async Task<ActionResult<AssetResponseDto>> Update(
        Guid id,
        [FromForm] UpdateAssetDto updateAssetDto)
    {
        return (await _assetService.UpdateAsync(id, updateAssetDto)).ToActionResult();
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Delete asset",
        Description = "Deletes an existing asset.")]
    [SwaggerResponse(204, "Asset deleted successfully", typeof(void))]
    public async Task<ActionResult> Delete(Guid id)
    {
        return (await _assetService.DeleteAsync(id)).ToActionResult();
    }
}