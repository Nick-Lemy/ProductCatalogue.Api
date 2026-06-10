using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;
using ProductCatalogue.Api.Services;

[ApiController]
[Route("variants")]
public class VariantController : ControllerBase
{
    private readonly IVariantService _variantService;
    public VariantController(IVariantService variantService)
    {
        _variantService = variantService;
    }

    [HttpPost]
    public async Task<ActionResult<Variant>> Create([FromBody] CreateVariantDto createVariantDto)
    {
        Variant newVariant = await _variantService.CreateAsync(createVariantDto);
        return CreatedAtAction(nameof(GetById), new { Id = newVariant.Id }, newVariant);
    }

    [HttpGet]
    public async Task<ActionResult<List<Variant>>> GetAll([FromQuery] VariantQueryDto query)
    {
        List<Variant> variants = await _variantService.GetAllAsync(query);
        return Ok(variants);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Variant>> GetById(Guid id)
    {
        Variant? variant = await _variantService.GetByIdAsync(id);
        return variant is null ? NotFound() : Ok(variant);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Variant>> Update(Guid id, [FromBody] UpdateVariantDto updateVariantDto)
    {
        Variant? updatedVariant = await _variantService.UpdateAsync(id, updateVariantDto);
        return updatedVariant is null ? NotFound() : Ok(updatedVariant);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        bool isDeleted = await _variantService.DeleteAsync(id);
        return !isDeleted ? NotFound() : NoContent();
    }
}