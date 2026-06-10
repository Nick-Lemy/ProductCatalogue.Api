using Microsoft.EntityFrameworkCore;
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
            new Product { Id = Guid.NewGuid(), Name = "Shirt", Brand = "Nike", Category = "Tops", ProductCode = "SHT-001" },
            new Product { Id = Guid.NewGuid(), Name = "Pants", Brand = "Adidas", Category = "Bottoms", ProductCode = "PNT-001" }
        );
        await context.SaveChangesAsync();

        var service = new ProductService(context);

        var result = await service.GetAllAsync(new ProductQueryDto());

        Assert.Equal(2, result.Count);
    }

}