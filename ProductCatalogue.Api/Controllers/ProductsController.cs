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
        List<ProductResponseDto> products = await _productService.GetAllAsync(query);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponseDto>> GetById(Guid id)
    {
        ProductResponseDto? product = await _productService.GetByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
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
        ProductResponseDto? updatedProduct = await _productService.UpdateAsync(id, updateProductDto);
        return updatedProduct is null ? NotFound() : Ok(updatedProduct);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        bool isDeleted = await _productService.DeleteAsync(id);
        return isDeleted ? NoContent() : NotFound();
    }
}
