using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductCatalogue.Api.Data;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Infrastructure.Messaging;
using ProductCatalogue.Api.Models;
using ProductCatalogue.Api.Settings;
using ProductCatalogue.Contracts;

namespace ProductCatalogue.Api.Services;

public class ProductService(
    AppDbContext context,
    IEventPublisher eventPublisher,
    IOptions<KafkaSettings> kafkaSettings,
    ILogger<ProductService> logger) : IProductService
{
    private readonly AppDbContext _context = context;
    private readonly IEventPublisher _eventPublisher = eventPublisher;
    private readonly string _productEventsTopic = kafkaSettings.Value.ProductEventsTopic;
    private readonly ILogger<ProductService> _logger = logger;

    public async Task<Result<ProductResponseDto>> CreateAsync(CreateProductDto createProductDto)
    {
        _logger.LogInformation("[Product] Creating product with name {Name}", createProductDto.Name);
        Product? productWithSameCode = await _context.Products.AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductCode == createProductDto.ProductCode);

        if (productWithSameCode is not null)
            return Result.Conflict($"Product with code {createProductDto.ProductCode} already exists");

        Product newProduct = createProductDto.Adapt<Product>();
        _context.Products.Add(newProduct);
        await _context.SaveChangesAsync();
        _logger.LogInformation("[Product] Product {ProductId} created successfully", newProduct.Id);
        return Result<ProductResponseDto>.Success(newProduct.Adapt<ProductResponseDto>());
    }

    public async Task<Result<List<ProductResponseDto>>> GetAllAsync(ProductQueryDto query)
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
        return Result<List<ProductResponseDto>>.Success(products.Adapt<List<ProductResponseDto>>());
    }

    public async Task<Result<ProductResponseDto>> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("[Product] Fetching product with id {Id}", id);

        Product? product = await _context.Products.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
            return Result.NotFound($"Product with id {id} not found");

        _logger.LogInformation("[Product] Product fetched successfully with id {Id}", id);
        return Result<ProductResponseDto>.Success(product.Adapt<ProductResponseDto>());
    }

    public async Task<Result<ProductResponseDto>> ChangeStatusAsync(Guid id, ChangeStatusDto changeStatusDto)
    {
        _logger.LogInformation("[Product] Changing status of product with id {id}", id);
        Product? product = await _context.Products.FindAsync(id);
        if (product is null)
            return Result.NotFound($"Product with id {id} not found");

        if (changeStatusDto.Status is ProductStatus.IN_REVIEW or ProductStatus.PUBLISHED)
            return Result.Validation($"Use the dedicated endpoint to set status {changeStatusDto.Status}");

        if (product.Status == changeStatusDto.Status)
            return Result.Conflict($"Product with id {id} already has status {changeStatusDto.Status}");

        product.Status = changeStatusDto.Status;
        await _context.SaveChangesAsync();
        _logger.LogInformation("[Product] Changed status of product with id {id} to {status}", id, product.Status);
        return Result<ProductResponseDto>.Success(product.Adapt<ProductResponseDto>());
    }

    public async Task<Result<ProductResponseDto>> SubmitForReviewAsync(Guid id)
    {
        _logger.LogInformation("[Product] Submitting product {id} for review", id);
        Product? product = await _context.Products.FindAsync(id);
        if (product is null)
            return Result.NotFound($"Product with id {id} not found");

        if (product.Status == ProductStatus.IN_REVIEW)
            return Result.Conflict($"Product with id {id} is already in review");

        await using var transaction = await _context.Database.BeginTransactionAsync();
        product.Status = ProductStatus.IN_REVIEW;
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        await _eventPublisher.PublishAsync(
            _productEventsTopic,
            product.Id.ToString(),
            new EventEnvelope<ProductSubmittedForReviewPayload>
            {
                EventType = EventTypes.ProductSubmittedForReview,
                Payload = new ProductSubmittedForReviewPayload(product.Id, product.ProductCode, product.Name)
            });

        _logger.LogInformation("[Product] Product {id} submitted for review", id);
        return Result<ProductResponseDto>.Success(product.Adapt<ProductResponseDto>());
    }

    public async Task<Result<ProductResponseDto>> PublishAsync(Guid id)
    {
        _logger.LogInformation("[Product] Publishing product {id}", id);
        Product? product = await _context.Products.FindAsync(id);
        if (product is null)
            return Result.NotFound($"Product with id {id} not found");

        if (product.Status == ProductStatus.PUBLISHED)
            return Result.Conflict($"Product with id {id} is already published");

        if (product.Readiness == ProductReadiness.NOT_READY)
            return Result.Conflict("Product must be ready before publishing");

        await using var transaction = await _context.Database.BeginTransactionAsync();
        product.Status = ProductStatus.PUBLISHED;
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        await _eventPublisher.PublishAsync(
            _productEventsTopic,
            product.Id.ToString(),
            new EventEnvelope<ProductPublishedPayload>
            {
                EventType = EventTypes.ProductPublished,
                Payload = new ProductPublishedPayload(product.Id, product.ProductCode, product.Name)
            });

        _logger.LogInformation("[Product] Product {id} published", id);
        return Result<ProductResponseDto>.Success(product.Adapt<ProductResponseDto>());
    }
    public async Task<Result<ProductResponseDto>> UpdateAsync(Guid id, UpdateProductDto updateProductDto)
    {
        _logger.LogInformation("[Product] Updating product with id {Id}", id);
        var product = await _context.Products.FindAsync(id);

        if (product is null)
            return Result.NotFound($"Product with id {id} not found");

        if (updateProductDto.ProductCode is not null && product.ProductCode != updateProductDto.ProductCode)
        {
            Product? productWithSameCode = await _context.Products.AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductCode == updateProductDto.ProductCode && p.Id != id);

            if (productWithSameCode is not null)
                return Result.Conflict($"Product with code {updateProductDto.ProductCode} already exists");
        }

        updateProductDto.Adapt(product);
        product.UpdatedAt = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync();
        _logger.LogInformation("[Product] Updated product with id {Id}", id);
        return Result<ProductResponseDto>.Success(product.Adapt<ProductResponseDto>());
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        _logger.LogInformation("[Product] Deleting product with id {Id}", id);
        var product = await _context.Products.FindAsync(id);

        if (product is null)
            return Result.NotFound($"Product with id {id} not found");
    
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        _logger.LogInformation("[Product] Deleted product with id {Id}", id);
        return Result.Success();
    }

}