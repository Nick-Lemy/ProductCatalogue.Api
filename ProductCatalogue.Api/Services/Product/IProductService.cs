using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public interface IProductService
{
    public Task<List<Product>> GetAllAsync(ProductQueryDto query);
    public Task<Product?> GetByIdAsync(Guid id);
    public Task<Product> CreateAsync(CreateProductDto createProductDto);
    public Task<Product?> UpdateAsync(Guid id, UpdateProductDto updateProductDto);
    public Task<bool> Delete(Guid id);
}