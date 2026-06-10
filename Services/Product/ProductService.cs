using Mapster;
using Microsoft.EntityFrameworkCore;
using ProductCatalogue.Api.Data;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product> CreateAsync(CreateProductDto createProductDto)
    {
        Product newProduct = createProductDto.Adapt<Product>();
        _context.Products.Add(newProduct);
        await _context.SaveChangesAsync();
        return newProduct;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        List<Product> products = await _context.Products.ToListAsync();
        return products;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        Product? product = await _context.Products.FindAsync(id);
        return product;
    }

    public async Task<Product?> UpdateAsync(Guid id, UpdateProductDto updateProductDto)
    {
        await Task.Delay(200);
        var product = await _context.Products.FindAsync(id);
        if (product is null) return null;
        updateProductDto.Adapt(product);
        product.UpdatedAt = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> Delete(Guid id)
    {
        var product = await GetByIdAsync(id);
        if (product is null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}