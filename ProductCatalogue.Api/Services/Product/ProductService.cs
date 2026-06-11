using Mapster;
using Microsoft.EntityFrameworkCore;
using ProductCatalogue.Api.Data;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public class ProductService(AppDbContext context) : IProductService
{
    private readonly AppDbContext _context = context;

    public async Task<ProductResponseDto> CreateAsync(CreateProductDto createProductDto)
    {
        Product newProduct = createProductDto.Adapt<Product>();
        _context.Products.Add(newProduct);
        await _context.SaveChangesAsync();
        return newProduct.Adapt<ProductResponseDto>();
    }

    public async Task<List<ProductResponseDto>> GetAllAsync(ProductQueryDto query)
    {
        var products = await _context.Products
            .Where(p => query.Name == null || p.Name.Contains(query.Name, StringComparison.CurrentCultureIgnoreCase))
            .Where(p => query.Brand == null || p.Brand == query.Brand)
            .Where(p => query.Category == null || p.Category == query.Category)
            .Where(p => query.Status == null || p.Status.ToString() == query.Status)
            .Where(p => query.Readiness == null || p.Readiness.ToString() == query.Readiness)
            .Where(p => query.ProductCode == null || p.ProductCode == query.ProductCode)
            .ToListAsync();

        return products.Adapt<List<ProductResponseDto>>();
    }

    public async Task<ProductResponseDto?> GetByIdAsync(Guid id)
    {
        Product? product = await _context.Products.FindAsync(id);
        return product?.Adapt<ProductResponseDto>();
    }

    public async Task<ProductResponseDto?> UpdateAsync(Guid id, UpdateProductDto updateProductDto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null) return null;
        updateProductDto.Adapt(product);
        product.UpdatedAt = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync();
        return product.Adapt<ProductResponseDto>();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}