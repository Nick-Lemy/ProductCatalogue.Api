using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductCatalogue.Api.Controllers;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;
using ProductCatalogue.Api.Services;

namespace ProductCatalogue.Tests.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<IProductService> _serviceMock;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _serviceMock = new Mock<IProductService>();
        _controller = new ProductsController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAll_Returns200_WithProducts()
    {
        var products = new List<ProductResponseDto>
        {
            new() { Name = "Shirt", Brand = "Nike", Category = "Tops", ProductCode = "SHT-001", Description = "A comfortable shirt", TargetMarket = " Men", Season = "Summer", Status = ProductStatus.DRAFT, Readiness = ProductReadiness.NOT_READY, CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow },
            new() { Name = "Pants", Brand = "Adidas", Category = "Bottoms", ProductCode = "PNT-001", Description = "Comfortable pants", TargetMarket = "Women", Season = "Winter", Status = ProductStatus.DRAFT, Readiness = ProductReadiness.NOT_READY, CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow }
        };

        var query = new ProductQueryDto()
        {
            Brand = "Nike",
            Category = "Tops",
            Status = ProductStatus.DRAFT,
            Readiness = ProductReadiness.NOT_READY
        };

        _serviceMock.Setup(s => s.GetAllAsync(It.IsAny<ProductQueryDto>()))
            .ReturnsAsync(products);

        var actionResult = await _controller.GetAll(query);

        var returned = Assert.IsType<List<ProductResponseDto>>(actionResult.Value, exactMatch: false);

        Assert.Equal(2, returned.Count);
    }

    [Fact]
    public async Task GetById_Returns200_WithProduct()
    {
        var productId = Guid.NewGuid();
        var product = new ProductResponseDto { Id = productId, Name = "Shirt", Brand = "Nike", Category = "Tops", ProductCode = "SHT-001", Description = "A comfortable shirt", TargetMarket = "Men", Season = "Summer", Status = ProductStatus.DRAFT, Readiness = ProductReadiness.NOT_READY, CreatedAt = DateTimeOffset.UtcNow, UpdatedAt = DateTimeOffset.UtcNow }; 

        _serviceMock.Setup(s => s.GetByIdAsync(productId))
            .ReturnsAsync(product);
        
        var actionResult = await _controller.GetById(productId);

        var returned = Assert.IsType<ProductResponseDto>(actionResult.Value, exactMatch: false);
        Assert.Equal(productId, returned.Id);
        Assert.Equal("Shirt", returned.Name);
        Assert.Equal("Nike", returned.Brand);
        Assert.Equal("Tops", returned.Category);
        Assert.Equal("SHT-001", returned.ProductCode);
        Assert.Equal("A comfortable shirt", returned.Description);
        Assert.Equal("Men", returned.TargetMarket);
        Assert.Equal("Summer", returned.Season);
        Assert.Equal(ProductStatus.DRAFT, returned.Status);
        Assert.Equal(ProductReadiness.NOT_READY, returned.Readiness);
    }

}
