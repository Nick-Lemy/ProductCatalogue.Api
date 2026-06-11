using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;
using ProductCatalogue.Api.Services;

namespace ProductCatalogue.Api.Controllers;

[ApiController]
[Route("products")]
public class ProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpGet]
    public async Task<ActionResult<List<ProductResponseDto>>> GetAll(
        [FromQuery] ProductQueryDto query
        )
    {
        return await _productService.GetAllAsync(query);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponseDto>> GetById(Guid id)
    {
        return await _productService.GetByIdAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> Create(
        [FromBody] CreateProductDto createProductDto
    )
    {
        ProductResponseDto newProduct = await _productService.CreateAsync(createProductDto);
        return CreatedAtAction(nameof(GetById), new { Id = newProduct.Id }, newProduct);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductResponseDto>> Update(Guid id, [FromBody] UpdateProductDto updateProductDto)
    {
        return await _productService.UpdateAsync(id, updateProductDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _productService.DeleteAsync(id);
        return NoContent();
    }
}
