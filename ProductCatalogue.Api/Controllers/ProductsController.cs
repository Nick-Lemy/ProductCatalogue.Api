using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Extensions;
using ProductCatalogue.Api.Models;
using ProductCatalogue.Api.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductCatalogue.Api.Controllers;

[ApiController]
[Authorize]
[Route("products")]
public class ProductsController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all products",
        Description = "Fetches a list of products based on optional query parameters for filtering.")]
    [SwaggerResponse(200, "List of products fetched successfully", typeof(List<ProductResponseDto>))]
    public async Task<ActionResult<List<ProductResponseDto>>> GetAll(
        [FromQuery] ProductQueryDto query
        )
    {
        return (await _productService.GetAllAsync(query)).ToActionResult();
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get product by ID",
        Description = "Fetches a product by its id.")]
    [SwaggerResponse(200, "Product fetched successfully", typeof(ProductResponseDto))]
    [SwaggerResponse(404, "Product not found")]
    public async Task<ActionResult<ProductResponseDto>> GetById(Guid id)
    {
        return (await _productService.GetByIdAsync(id)).ToActionResult();
    }

    [HttpPatch("{id}/change-status")]
    [SwaggerOperation(
        Summary = "Change product status",
        Description = "Change status of a product by its id")]
    [SwaggerResponse(200, "Status changed successfully", typeof(ProductResponseDto))]
    [SwaggerResponse(400, "Invalide status value")]
    [SwaggerResponse(404, "Product not found")]
    public async Task<ActionResult<ProductResponseDto>> ChangeStatus(
        Guid id,
        [FromBody] ChangeStatusDto changeStatusDto)
    {
        return (await _productService.ChangeStatusAsync(id, changeStatusDto)).ToActionResult();
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create product",
        Description = "Creates a new product with the provided details.")]
    [SwaggerResponse(201, "Product created successfully", typeof(ProductResponseDto))]
    [SwaggerResponse(400, "Invalid product data")]
    public async Task<ActionResult<ProductResponseDto>> Create(
        [FromBody] CreateProductDto createProductDto
    )
    {
        var result = await _productService.CreateAsync(createProductDto);
        if (!result.IsSuccess)
            return result.ToActionResult();

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(
        Summary = "Update product",
        Description = "Updates an existing product with the provided details.")]
    [SwaggerResponse(200, "Product updated successfully", typeof(ProductResponseDto))]
    [SwaggerResponse(404, "Product not found")]
    public async Task<ActionResult<ProductResponseDto>> Update(Guid id, [FromBody] UpdateProductDto updateProductDto)
    {
        return (await _productService.UpdateAsync(id, updateProductDto)).ToActionResult();
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = "Delete product",
        Description = "Deletes a product by its id.")]
    [SwaggerResponse(204, "Product deleted successfully")]
    [SwaggerResponse(404, "Product not found")]
    public async Task<ActionResult> Delete(Guid id)
    {
        return (await _productService.DeleteAsync(id)).ToActionResult();
    }
}
