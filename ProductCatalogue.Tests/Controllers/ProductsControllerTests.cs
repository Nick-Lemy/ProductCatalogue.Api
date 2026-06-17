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

        var returned = Assert.IsAssignableFrom<List<ProductResponseDto>>(actionResult.Value);

        Assert.Equal(2, returned.Count);
    }

}
