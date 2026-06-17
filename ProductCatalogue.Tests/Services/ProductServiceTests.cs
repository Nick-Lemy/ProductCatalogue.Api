using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductCatalogue.Api.Data;
using ProductCatalogue.Api.DTOs;
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

}