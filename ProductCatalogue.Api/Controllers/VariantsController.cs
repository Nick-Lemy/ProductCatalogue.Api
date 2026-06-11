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
        return await _variantService.GetAllAsync(query);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VariantReponseDto>> GetById(Guid id)
    {
        return await _variantService.GetByIdAsync(id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<VariantReponseDto>> Update(
        Guid id,
        [FromBody] UpdateVariantDto updateVariantDto
        )
    {
        return await _variantService.UpdateAsync(id, updateVariantDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _variantService.DeleteAsync(id);
        return NoContent();
    }
}