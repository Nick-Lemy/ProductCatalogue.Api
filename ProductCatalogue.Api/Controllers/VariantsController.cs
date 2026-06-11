using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;
using ProductCatalogue.Api.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductCatalogue.Api.Controllers;

[ApiController]
[Route("variants")]
public class VariantController(IVariantService variantService) : ControllerBase
{
    private readonly IVariantService _variantService = variantService;

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create variant",
        Description = "Creates a new variant with the provided details.")]
    [SwaggerResponse(201, "Variant created successfully", typeof(VariantReponseDto))]
    [SwaggerResponse(400, "Invalid variant data")]
    public async Task<ActionResult<VariantReponseDto>> Create([FromBody] CreateVariantDto createVariantDto)
    {
        VariantReponseDto newVariant = await _variantService.CreateAsync(createVariantDto);
        return CreatedAtAction(nameof(GetById), new { Id = newVariant.Id }, newVariant);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all variants",
        Description = "Fetches a list of variants based on optional query parameters for filtering.")]
    [SwaggerResponse(200, "List of variants fetched successfully", typeof(List<VariantReponseDto>))]
    public async Task<ActionResult<List<VariantReponseDto>>> GetAll([FromQuery] VariantQueryDto query)
    {
        return await _variantService.GetAllAsync(query);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get variant by ID",
        Description = "Fetches a variant by its id.")]
    [SwaggerResponse(200, "Variant fetched successfully", typeof(VariantReponseDto))]
    [SwaggerResponse(404, "Variant not found")]
    public async Task<ActionResult<VariantReponseDto>> GetById(Guid id)
    {
        return await _variantService.GetByIdAsync(id);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Update variant",
        Description = "Updates an existing variant with the provided details.")]
    [SwaggerResponse(200, "Variant updated successfully", typeof(VariantReponseDto))]
    [SwaggerResponse(404, "Variant not found")]
    public async Task<ActionResult<VariantReponseDto>> Update(
        Guid id,
        [FromBody] UpdateVariantDto updateVariantDto
        )
    {
        return await _variantService.UpdateAsync(id, updateVariantDto);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Delete variant",
        Description = "Deletes a variant by its id.")]
    [SwaggerResponse(204, "Variant deleted successfully")]
    [SwaggerResponse(404, "Variant not found")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _variantService.DeleteAsync(id);
        return NoContent();
    }
}