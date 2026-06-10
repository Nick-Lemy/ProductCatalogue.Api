using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;
using ProductCatalogue.Api.Services;

namespace ProductCatalogue.Api.Controllers;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAll(
        [FromQuery] ProductQueryDto query
        )
    {
        List<Product> products = await _productService.GetAllAsync(query);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(Guid id)
    {
        Product? product = await _productService.GetByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(
        [FromBody] CreateProductDto createProductDto
    )
    {
        Product newProduct = await _productService.CreateAsync(createProductDto);
        return CreatedAtAction(nameof(GetById), new { Id = newProduct.Id }, newProduct);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Product>> Update(Guid id, [FromBody] UpdateProductDto updateProductDto)
    {
        Product? updatedProduct = await _productService.UpdateAsync(id, updateProductDto);
        return updatedProduct is null ? NotFound() : Ok(updatedProduct);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        bool isDeleted = await _productService.Delete(id);
        if (!isDeleted) BadRequest();
        return NoContent();
    }
}
