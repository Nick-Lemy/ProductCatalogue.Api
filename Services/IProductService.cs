using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public interface IProductService
{
    public Task<List<Product>> GetAllAsync();
    public Task<Product?> GetByIdAsync(int id);
    public Task<Product> CreateAsync(CreateProductDto createProductDto);
    public Task<Product?> UpdateAsync(int id, UpdateProductDto updateProductDto);
    public Task<bool> Delete(int id);
}