using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductCatalogue.Api.Controllers;

[ApiController]
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
        return await _assetService.GetAllAsync(query);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get asset by ID",
        Description = "Fetches an asset by its unique identifier.")]
    [SwaggerResponse(200, "Asset fetched successfully", typeof(AssetResponseDto))]
    public async Task<ActionResult<AssetResponseDto>> GetById(Guid id)
    {
        return await _assetService.GetByIdAsync(id);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create asset",
        Description = "Creates a new asset for a product.")]
    [SwaggerResponse(201, "Asset created successfully", typeof(AssetResponseDto))]
    public async Task<ActionResult<AssetResponseDto>> Create([FromForm] UploadAssetDto uploadAssetDto)
    {
        AssetResponseDto newAsset = await _assetService.CreateAsync(uploadAssetDto);
        return CreatedAtAction(nameof(GetById), new { id = newAsset.Id }, newAsset);
    }
    
    [HttpPost("{id}/reject")]
    [SwaggerOperation(
        Summary = "Reject asset",
        Description = "Rejects an existing asset.")]
    [SwaggerResponse(204, "Asset rejected successfully", typeof(void))]
    public async Task<ActionResult> Reject(Guid id, [FromBody] RejectAssetDto rejectAssetDto)
    {
        await _assetService.RejectAssetAsync(id, rejectAssetDto);
        return NoContent();
    }

    [HttpPost("{id}/approve")]
    [SwaggerOperation(
        Summary = "Approve asset",
        Description = "Approves an existing asset.")]
    [SwaggerResponse(204, "Asset approved successfully", typeof(void))]
    public async Task<ActionResult> Approve(
        Guid id,
        [FromBody] ApproveAssetDto approveAssetDto)
    {
        await _assetService.ApproveAssetAsync(id, approveAssetDto);
        return NoContent();
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
        await _assetService.UpdateAsync(id, updateAssetDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Delete asset",
        Description = "Deletes an existing asset.")]
    [SwaggerResponse(204, "Asset deleted successfully", typeof(void))]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _assetService.DeleteAsync(id);
        return NoContent();
    }
}