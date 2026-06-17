using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductCatalogue.Api.Data;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Exceptions;
using ProductCatalogue.Api.Models;
using ProductCatalogue.Api.Services;

namespace ProductCatalogue.Tests.Services;

public class ProductServiceTests
{
    private AppDbContext CreateContext() =>
        new(new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    [Fact]
    public async Task GetAllAsync_ReturnsAllProducts_WhenNoFiltersApplied()
    {
        var context = CreateContext();
        context.Products.AddRange(
            new Product { Id = Guid.NewGuid(), Name = "Shirt", Brand = "Nike", Category = "Tops", ProductCode = "SHT-001", Description = "A comfortable shirt", TargetMarket = "Men", Season = "Summer" },
            new Product { Id = Guid.NewGuid(), Name = "Pants", Brand = "Adidas", Category = "Bottoms", ProductCode = "PNT-001", Description = "Comfortable pants", TargetMarket = "Women", Season = "Winter" }
        );
        await context.SaveChangesAsync();

        var logger = new LoggerFactory().CreateLogger<ProductService>();
        var service = new ProductService(context, logger);

        var result = await service.GetAllAsync(new ProductQueryDto());

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsFilteredProducts_WhenFiltersApplied()
    {
        var context = CreateContext();
        context.Products.AddRange(
            new Product { Id = Guid.NewGuid(), Name = "Shirt", Brand = "Nike", Category = "Tops", ProductCode = "SHT-001", Description = "A comfortable shirt", TargetMarket = "Men", Season = "Summer" },
            new Product { Id = Guid.NewGuid(), Name = "Pants", Brand = "Adidas", Category = "Bottoms", ProductCode = "PNT-001", Description = "Comfortable pants", TargetMarket = "Women", Season = "Winter" }
        );
        await context.SaveChangesAsync();

        var logger = new LoggerFactory().CreateLogger<ProductService>();
        var service = new ProductService(context, logger);

        var result = await service.GetAllAsync(new ProductQueryDto { Brand = "Nike" });

        Assert.Single(result);
        Assert.Equal("Nike", result.First().Brand);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoProductsMatchFilters()
    {
        var context = CreateContext();
        context.Products.AddRange(
            new Product { Id = Guid.NewGuid(), Name = "Shirt", Brand = "Nike", Category = "Tops", ProductCode = "SHT-001", Description = "A comfortable shirt", TargetMarket = "Men", Season = "Summer" },
            new Product { Id = Guid.NewGuid(), Name = "Pants", Brand = "Adidas", Category = "Bottoms", ProductCode = "PNT-001", Description = "Comfortable pants", TargetMarket = "Women", Season = "Winter" }
        );
        await context.SaveChangesAsync();

        var logger = new LoggerFactory().CreateLogger<ProductService>();
        var service = new ProductService(context, logger);

        var result = await service.GetAllAsync(new ProductQueryDto { Brand = "Reebok" });

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsProduct_WhenProductExists()
    {
        var context = CreateContext();
        var productId = Guid.NewGuid();
        context.Products.Add(new Product { Id = productId, Name = "Shirt", Brand = "Nike", Category = "Tops", ProductCode = "SHT-001", Description = "A comfortable shirt", TargetMarket = "Men", Season = "Summer" });
        await context.SaveChangesAsync();

        var logger = new LoggerFactory().CreateLogger<ProductService>();
        var service = new ProductService(context, logger);

        var result = await service.GetByIdAsync(productId);

        Assert.NotNull(result);
        Assert.Equal("Shirt", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsNotFoundException_WhenProductDoesNotExist()
    {
        var context = CreateContext();
        var logger = new LoggerFactory().CreateLogger<ProductService>();
        var service = new ProductService(context, logger);

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task CreateAsync_AddsProductToDatabase()
    {
        var context = CreateContext();
        var logger = new LoggerFactory().CreateLogger<ProductService>();
        var service = new ProductService(context, logger);

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
    public async Task CreateAsync_ThrowsConflictException_WhenProductCodeAlreadyExists()
    {
        var context = CreateContext();
        context.Products.Add(new Product { Id = Guid.NewGuid(), Name = "Shirt", Brand = "Nike", Category = "Tops", ProductCode = "SHT-001", Description = "A comfortable shirt", TargetMarket = "Men", Season = "Summer" });
        await context.SaveChangesAsync();

        var logger = new LoggerFactory().CreateLogger<ProductService>();
        var service = new ProductService(context, logger);

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

        await Assert.ThrowsAsync<ConflictException>(async () => await service.CreateAsync(productDto));
    }

    [Fact]
    public async Task ChangeStatusAsync_ChangesProductStatus_WhenProductExists()
    {
        var context = CreateContext();
        var productId = Guid.NewGuid();
        context.Products.Add(new Product { Id = productId, Name = "Shirt", Brand = "Nike", Category = "Tops", ProductCode = "SHT-001", Description = "A comfortable shirt", TargetMarket = "Men", Season = "Summer", Status = ProductStatus.DRAFT });
        await context.SaveChangesAsync();

        var logger = new LoggerFactory().CreateLogger<ProductService>();
        var service = new ProductService(context, logger);

        var changeStatusDto = new ChangeStatusDto
        {
            Status = ProductStatus.IN_REVIEW
        };

        await service.ChangeStatusAsync(productId, changeStatusDto);

        var updatedProduct = await context.Products.FindAsync(productId);
        Assert.Equal(ProductStatus.IN_REVIEW, updatedProduct?.Status);
    }

    [Fact]
    public async Task ChangeStatusAsync_ThrowsNotFoundException_WhenProductDoesNotExist()
    {
        var context = CreateContext();
        var logger = new LoggerFactory().CreateLogger<ProductService>();
        var service = new ProductService(context, logger);

        var changeStatusDto = new ChangeStatusDto
        {
            Status = ProductStatus.IN_REVIEW
        };

        await Assert.ThrowsAsync<NotFoundException>(async () => await service.ChangeStatusAsync(Guid.NewGuid(), changeStatusDto));
    }

    [Fact]
    public async Task ChangeStatusAsync_ThrowsConflictException_WhenProductAlreadyHasStatus()
    {
        var context = CreateContext();
        var productId = Guid.NewGuid();
        context.Products.Add(new Product { Id = productId, Name = "Shirt", Brand = "Nike", Category = "Tops", ProductCode = "SHT-001", Description = "A comfortable shirt", TargetMarket = "Men", Season = "Summer", Status = ProductStatus.DRAFT });
        await context.SaveChangesAsync();

        var logger = new LoggerFactory().CreateLogger<ProductService>();
        var service = new ProductService(context, logger);

        var changeStatusDto = new ChangeStatusDto
        {
            Status = ProductStatus.DRAFT
        };

        await Assert.ThrowsAsync<ConflictException>(async () => await service.ChangeStatusAsync(productId, changeStatusDto));
    }

    [Fact]
    public async Task ChangeStatusAsync_ThrowsConflictException_WhenChangingToPublishedAndNotReady()
    {
        var context = CreateContext();
        var productId = Guid.NewGuid();
        context.Products.Add(new Product { Id = productId, Name = "Shirt", Brand = "Nike", Category = "Tops", ProductCode = "SHT-001", Description = "A comfortable shirt", TargetMarket = "Men", Season = "Summer", Status = ProductStatus.DRAFT, Readiness = ProductReadiness.NOT_READY });
        await context.SaveChangesAsync();

        var logger = new LoggerFactory().CreateLogger<ProductService>();
        var service = new ProductService(context, logger);

        var changeStatusDto = new ChangeStatusDto
        {
            Status = ProductStatus.PUBLISHED
        };

        await Assert.ThrowsAsync<ConflictException>(async () => await service.ChangeStatusAsync(productId, changeStatusDto));
    }

    [Fact]
    public async Task UpdateAsync_UpdatesProduct_WhenProductExists()
    {
        var context = CreateContext();
        var productId = Guid.NewGuid();
        context.Products.Add(new Product { Id = productId, Name = "Shirt", Brand = "Nike", Category = "Tops", ProductCode = "SHT-001", Description = "A comfortable shirt", TargetMarket = "Men", Season = "Summer" });
        await context.SaveChangesAsync();

        var logger = new LoggerFactory().CreateLogger<ProductService>();
        var service = new ProductService(context, logger);

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

        await service.UpdateAsync(productId, updateProductDto);

        var updatedProduct = await context.Products.FindAsync(productId);
        Assert.Equal("Updated Shirt", updatedProduct?.Name);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsNotFoundException_WhenProductDoesNotExist()
    {
        var context = CreateContext();
        var logger = new LoggerFactory().CreateLogger<ProductService>();
        var service = new ProductService(context, logger);

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

        await Assert.ThrowsAsync<NotFoundException>(async () => await service.UpdateAsync(Guid.NewGuid(), updateProductDto));
    }

    [Fact]
    public async Task UpdateAsync_ThrowsConflictException_WhenProductCodeAlreadyExists()
    {
        var context = CreateContext();
        var existingProductId = Guid.NewGuid();
        context.Products.AddRange(
            new Product { Id = existingProductId, Name = "Shirt", Brand = "Nike", Category = "Tops", ProductCode = "SHT-001", Description = "A comfortable shirt", TargetMarket = "Men", Season = "Summer" },
            new Product { Id = Guid.NewGuid(), Name = "Pants", Brand = "Adidas", Category = "Bottoms", ProductCode = "PNT-001", Description = "Comfortable pants", TargetMarket = "Women", Season = "Winter" }
        );
        await context.SaveChangesAsync();

        var logger = new LoggerFactory().CreateLogger<ProductService>();
        var service = new ProductService(context, logger);

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

        await Assert.ThrowsAsync<ConflictException>(async () => await service.UpdateAsync(existingProductId, updateProductDto));
    }

    [Fact]
    public async Task DeleteAsync_DeletesProduct_WhenProductExists()
    {
        var context = CreateContext();
        var productId = Guid.NewGuid();
        context.Products.Add(new Product { Id = productId, Name = "Shirt", Brand = "Nike", Category = "Tops", ProductCode = "SHT-001", Description = "A comfortable shirt", TargetMarket = "Men", Season = "Summer" });
        await context.SaveChangesAsync();

        var logger = new LoggerFactory().CreateLogger<ProductService>();
        var service = new ProductService(context, logger);

        await service.DeleteAsync(productId);

        var deletedProduct = await context.Products.FindAsync(productId);
        Assert.Null(deletedProduct);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsNotFoundException_WhenProductDoesNotExist()
    {
        var context = CreateContext();
        var logger = new LoggerFactory().CreateLogger<ProductService>();
        var service = new ProductService(context, logger);

        await Assert.ThrowsAsync<NotFoundException>(async () => await service.DeleteAsync(Guid.NewGuid()));
    }
}