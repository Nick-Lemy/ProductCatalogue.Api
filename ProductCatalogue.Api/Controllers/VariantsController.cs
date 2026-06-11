using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;
using ProductCatalogue.Api.Services;

namespace ProductCatalogue.Api.Controllers;

[ApiController]
[Route("variants")]
public class VariantController(IVariantService variantService) : ControllerBase
{
    private readonly IVariantService _variantService = variantService;

    [HttpPost]
    public async Task<ActionResult<VariantReponseDto>> Create([FromBody] CreateVariantDto createVariantDto)
    {
        VariantReponseDto newVariant = await _variantService.CreateAsync(createVariantDto);
        return CreatedAtAction(nameof(GetById), new { Id = newVariant.Id }, newVariant);
    }

    [HttpGet]
    public async Task<ActionResult<List<VariantReponseDto>>> GetAll([FromQuery] VariantQueryDto query)
    {
        List<VariantReponseDto> variants = await _variantService.GetAllAsync(query);
        return Ok(variants);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VariantReponseDto>> GetById(Guid id)
    {
        VariantReponseDto? variant = await _variantService.GetByIdAsync(id);
        return variant is null ? NotFound() : Ok(variant);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<VariantReponseDto>> Update(
        Guid id,
        [FromBody] UpdateVariantDto updateVariantDto
        )
    {
        VariantReponseDto? updatedVariant = await _variantService.UpdateAsync(id, updateVariantDto);
        return updatedVariant is null ? NotFound() : Ok(updatedVariant);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        bool isDeleted = await _variantService.DeleteAsync(id);
        return !isDeleted ? NotFound() : NoContent();
    }
}