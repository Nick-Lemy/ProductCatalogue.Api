using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public interface IProductService
{
    public Task<List<ProductResponseDto>> GetAllAsync(ProductQueryDto query);
    public Task<ProductResponseDto?> GetByIdAsync(Guid id);
    public Task<ProductResponseDto> CreateAsync(CreateProductDto createProductDto);
    public Task<ProductResponseDto?> UpdateAsync(Guid id, UpdateProductDto updateProductDto);
    public Task<bool> DeleteAsync(Guid id);
}