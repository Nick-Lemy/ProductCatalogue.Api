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
        var products = new List<Product>
        {
            new() { Name = "Shirt", Brand = "Nike" },
            new() { Name = "Pants", Brand = "Adidas" }
        };
        _serviceMock.Setup(s => s.GetAllAsync(It.IsAny<ProductQueryDto>()))
            .ReturnsAsync(products);

        var result = await _controller.GetAll(new ProductQueryDto());

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsType<List<Product>>(ok.Value);
        Assert.Equal(2, returned.Count);
    }

}
