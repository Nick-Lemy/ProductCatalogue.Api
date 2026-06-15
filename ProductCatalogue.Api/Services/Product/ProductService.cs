using Mapster;
using Microsoft.EntityFrameworkCore;
using ProductCatalogue.Api.Data;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Exceptions;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public class ProductService(
    AppDbContext context,
    ILogger<ProductService> logger) : IProductService
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<ProductService> _logger = logger;

    public async Task<ProductResponseDto> CreateAsync(CreateProductDto createProductDto)
    {
        _logger.LogInformation("[Product] Creating product with name {Name}", createProductDto.Name);
        Product? productWithSameCode = await _context.Products.AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductCode == createProductDto.ProductCode);

        if (productWithSameCode is not null)
            throw new ConflictException($"Product with code {createProductDto.ProductCode} already exists");

        Product newProduct = createProductDto.Adapt<Product>();
        _context.Products.Add(newProduct);
        await _context.SaveChangesAsync();
        _logger.LogInformation("[Product] Product {ProductId} created successfully", newProduct.Id);
        return newProduct.Adapt<ProductResponseDto>();
    }

    public async Task<List<ProductResponseDto>> GetAllAsync(ProductQueryDto query)
    {
        _logger.LogInformation("[Product] Fetching products with query {@Query}", query);
        List<Product> products = await _context.Products.AsNoTracking()
            .Where(p => query.Name == null || EF.Functions.ILike(p.Name, $"%{query.Name}%"))
            .Where(p => query.Brand == null || p.Brand == query.Brand)
            .Where(p => query.Category == null || p.Category == query.Category)
            .Where(p => query.Status == null || p.Status == query.Status)
            .Where(p => query.Readiness == null || p.Readiness == query.Readiness)
            .Where(p => query.ProductCode == null || p.ProductCode == query.ProductCode)
            .ToListAsync();

        _logger.LogInformation("[Product] Fetched {Count} products", products.Count);
        return products.Adapt<List<ProductResponseDto>>();
    }

    public async Task<ProductResponseDto> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("[Product] Fetching product with id {Id}", id);
        Product? product = await _context.Products.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
            throw new NotFoundException($"Product with id {id} not found");

        _logger.LogInformation("[Product] Product fetched successfully with id {Id}", id);
        return product.Adapt<ProductResponseDto>();
    }

    public async Task<ProductResponseDto> ChangeStatusAsync(Guid id, ChangeStatusDto changeStatusDto)
    {
        _logger.LogInformation("[Product] Changing status of product with id {id}", id);
        Product? product = await _context.Products.FindAsync(id);
        if (product is null)
            throw new NotFoundException($"Product with id {id} not found");

        if (product.Status == changeStatusDto.Status)
            throw new ConflictException($"Product with id {id} already has status {changeStatusDto.Status}");

        if (changeStatusDto.Status == ProductStatus.PUBLISHED && product.Readiness == ProductReadiness.NOT_READY)
            throw new ConflictException("Product must be ready before publishing");

        product.Status = changeStatusDto.Status;
        await _context.SaveChangesAsync();
        _logger.LogInformation("[Product] Changed status of product with id {id} to {status}", id, product.Status);
        return product.Adapt<ProductResponseDto>();
    }
    public async Task<ProductResponseDto> UpdateAsync(Guid id, UpdateProductDto updateProductDto)
    {
        _logger.LogInformation("[Product] Updating product with id {Id}", id);
        var product = await _context.Products.FindAsync(id);

        if (product is null)
            throw new NotFoundException($"Product with id {id} not found");

        if (updateProductDto.ProductCode is not null && product.ProductCode != updateProductDto.ProductCode)
        {
            Product? productWithSameCode = await _context.Products.AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductCode == updateProductDto.ProductCode && p.Id != id);

            if (productWithSameCode is not null)
                throw new ConflictException($"Product with code {updateProductDto.ProductCode} already exists");
        }

        updateProductDto.Adapt(product);
        product.UpdatedAt = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync();
        _logger.LogInformation("[Product] Updated product with id {Id}", id);
        return product.Adapt<ProductResponseDto>();
    }

    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("[Product] Deleting product with id {Id}", id);
        var product = await _context.Products.FindAsync(id);

        if (product is null)
            throw new NotFoundException($"Product with id {id} not found");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        _logger.LogInformation("[Product] Deleted product with id {Id}", id);
    }

}