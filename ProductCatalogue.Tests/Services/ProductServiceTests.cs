using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using ProductCatalogue.Api.Data;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Infrastructure.Messaging;
using ProductCatalogue.Api.Models;
using ProductCatalogue.Api.Services;
using ProductCatalogue.Api.Settings;

namespace ProductCatalogue.Tests.Services;

public class ProductServiceTests
{
    private AppDbContext CreateContext() =>
        new(new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options);

    private readonly Product _shirt = new()
    {
        Id = Guid.NewGuid(),
        Name = "Shirt",
        Brand = "Nike",
        Category = "Tops",
        ProductCode = "SHT-001",
        Description = "A comfortable shirt",
        TargetMarket = "Men",
        Season = "Summer",
        Status = ProductStatus.DRAFT,
        Readiness = ProductReadiness.NOT_READY
    };

    private readonly Product _pants = new()
    {
        Id = Guid.NewGuid(),
        Name = "Pants",
        Brand = "Adidas",
        Category = "Bottoms",
        ProductCode = "PNT-001",
        Description = "Comfortable pants",
        TargetMarket = "Women",
        Season = "Winter"
    };

    private static readonly IOptions<KafkaSettings> KafkaOptions = Options.Create(new KafkaSettings
    {
        BootstrapServers = "localhost:9092",
        AssetEventsTopic = "catalogue.asset-events",
        ProductEventsTopic = "catalogue.product-events"
    });

    private static ProductService CreateService(AppDbContext context) =>
        new(context,
            new Mock<IEventPublisher>().Object,
            KafkaOptions,
            new LoggerFactory().CreateLogger<ProductService>());

    [Fact]
    public async Task GetAllAsync_ReturnsAllProducts_WhenNoFiltersApplied()
    {
        var context = CreateContext();
        context.Products.AddRange(_shirt, _pants);
        await context.SaveChangesAsync();

        var service = CreateService(context);

        var result = await service.GetAllAsync(new ProductQueryDto());

        Assert.Equal(2, result.Value!.Count);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsFilteredProducts_WhenFiltersApplied()
    {
        var context = CreateContext();
        context.Products.AddRange(_shirt, _pants);
        await context.SaveChangesAsync();

        var service = CreateService(context);

        var result = await service.GetAllAsync(new ProductQueryDto { Brand = "Nike" });

        Assert.Single(result.Value!);
        Assert.Equal("Nike", result.Value!.First().Brand);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoProductsMatchFilters()
    {
        var context = CreateContext();
        context.Products.AddRange(_shirt, _pants);
        await context.SaveChangesAsync();

        var service = CreateService(context);

        var result = await service.GetAllAsync(new ProductQueryDto { Brand = "Reebok" });

        Assert.Empty(result.Value!);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsProduct_WhenProductExists()
    {
        var context = CreateContext();
        context.Products.Add(_shirt);
        await context.SaveChangesAsync();

        var service = CreateService(context);

        var result = await service.GetByIdAsync(_shirt.Id);

        Assert.True(result.IsSuccess);
        Assert.Equal("Shirt", result.Value!.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNotFound_WhenProductDoesNotExist()
    {
        var context = CreateContext();
        var service = CreateService(context);

        var result = await service.GetByIdAsync(Guid.NewGuid());

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.NotFound, result.Error!.Type);
    }

    [Fact]
    public async Task CreateAsync_AddsProductToDatabase()
    {
        var context = CreateContext();
        var service = CreateService(context);

        var productDto = new CreateProductDto
        {
            Name = "Shirt",
            Brand = "Nike",
            Category = "Tops",
            ProductCode = "SHT-001",
            Description = "A comfortable shirt",
            TargetMarket = "Men",
            Season = "Summer"
        };

        await service.CreateAsync(productDto);

        var product = await context.Products.FirstOrDefaultAsync(p => p.ProductCode == "SHT-001");
        Assert.NotNull(product);
    }

    [Fact]
    public async Task CreateAsync_ReturnsConflict_WhenProductCodeAlreadyExists()
    {
        var context = CreateContext();
        context.Products.Add(_shirt);
        await context.SaveChangesAsync();

        var service = CreateService(context);

        var productDto = new CreateProductDto
        {
            Name = "Another Shirt",
            Brand = "Adidas",
            Category = "Tops",
            ProductCode = "SHT-001",
            Description = "Another comfortable shirt",
            TargetMarket = "Men",
            Season = "Summer"
        };

        var result = await service.CreateAsync(productDto);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Conflict, result.Error!.Type);
    }

    [Fact]
    public async Task SubmitForReviewAsync_SetsStatusToInReview_WhenProductExists()
    {
        var context = CreateContext();
        context.Products.Add(_shirt);
        await context.SaveChangesAsync();

        var service = CreateService(context);

        await service.SubmitForReviewAsync(_shirt.Id);

        var updatedProduct = await context.Products.FindAsync(_shirt.Id);
        Assert.Equal(ProductStatus.IN_REVIEW, updatedProduct?.Status);
    }

    [Fact]
    public async Task ChangeStatusAsync_ReturnsNotFound_WhenProductDoesNotExist()
    {
        var context = CreateContext();
        var service = CreateService(context);

        var changeStatusDto = new ChangeStatusDto
        {
            Status = ProductStatus.IN_REVIEW
        };

        var result = await service.ChangeStatusAsync(Guid.NewGuid(), changeStatusDto);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.NotFound, result.Error!.Type);
    }

    [Fact]
    public async Task ChangeStatusAsync_ReturnsConflict_WhenProductAlreadyHasStatus()
    {
        var context = CreateContext();
        context.Products.Add(_shirt);
        await context.SaveChangesAsync();

        var service = CreateService(context);

        var changeStatusDto = new ChangeStatusDto
        {
            Status = ProductStatus.DRAFT
        };

        var result = await service.ChangeStatusAsync(_shirt.Id, changeStatusDto);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Conflict, result.Error!.Type);
    }

    [Fact]
    public async Task PublishAsync_ReturnsConflict_WhenProductNotReady()
    {
        var context = CreateContext();
        context.Products.Add(_shirt);
        await context.SaveChangesAsync();

        var service = CreateService(context);

        var result = await service.PublishAsync(_shirt.Id);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Conflict, result.Error!.Type);
    }

    [Fact]
    public async Task PublishAsync_SetsStatusToPublished_WhenProductReady()
    {
        var context = CreateContext();
        _shirt.Readiness = ProductReadiness.READY;
        context.Products.Add(_shirt);
        await context.SaveChangesAsync();

        var service = CreateService(context);

        await service.PublishAsync(_shirt.Id);

        var updatedProduct = await context.Products.FindAsync(_shirt.Id);
        Assert.Equal(ProductStatus.PUBLISHED, updatedProduct?.Status);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesProduct_WhenProductExists()
    {
        var context = CreateContext();
        context.Products.Add(_shirt);
        await context.SaveChangesAsync();

        var service = CreateService(context);

        var updateProductDto = new UpdateProductDto
        {
            Name = "Updated Shirt",
            Brand = "Nike",
            Category = "Tops",
            ProductCode = "SHT-001",
            Description = "An updated comfortable shirt",
            TargetMarket = "Men",
            Season = "Summer"
        };

        await service.UpdateAsync(_shirt.Id, updateProductDto);

        var updatedProduct = await context.Products.FindAsync(_shirt.Id);
        Assert.Equal("Updated Shirt", updatedProduct?.Name);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNotFound_WhenProductDoesNotExist()
    {
        var context = CreateContext();
        var service = CreateService(context);

        var updateProductDto = new UpdateProductDto
        {
            Name = "Updated Shirt",
            Brand = "Nike",
            Category = "Tops",
            ProductCode = "SHT-001",
            Description = "An updated comfortable shirt",
            TargetMarket = "Men",
            Season = "Summer"
        };

        var result = await service.UpdateAsync(Guid.NewGuid(), updateProductDto);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.NotFound, result.Error!.Type);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsConflict_WhenProductCodeAlreadyExists()
    {
        var context = CreateContext();
        context.Products.AddRange(_shirt, _pants);
        await context.SaveChangesAsync();

        var service = CreateService(context);

        var updateProductDto = new UpdateProductDto
        {
            Name = "Updated Shirt",
            Brand = "Nike",
            Category = "Tops",
            ProductCode = "PNT-001",
            Description = "An updated comfortable shirt",
            TargetMarket = "Men",
            Season = "Summer"
        };

        var result = await service.UpdateAsync(_shirt.Id, updateProductDto);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Conflict, result.Error!.Type);
    }

    [Fact]
    public async Task DeleteAsync_DeletesProduct_WhenProductExists()
    {
        var context = CreateContext();
        context.Products.Add(_shirt);
        await context.SaveChangesAsync();

        var service = CreateService(context);

        await service.DeleteAsync(_shirt.Id);

        var deletedProduct = await context.Products.FindAsync(_shirt.Id);
        Assert.Null(deletedProduct);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNotFound_WhenProductDoesNotExist()
    {
        var context = CreateContext();
        var service = CreateService(context);

        var result = await service.DeleteAsync(Guid.NewGuid());

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.NotFound, result.Error!.Type);
    }
}
