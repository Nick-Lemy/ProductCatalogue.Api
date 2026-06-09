using Mapster;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public class ProductService : IProductService
{
    private readonly List<Product> _products;

    public ProductService(List<Product> products)
    {
        _products = products;
    }

    public async Task<Product> CreateAsync(CreateProductDto createProductDto)
    {
        Product newProduct = createProductDto.Adapt<Product>();

        _products.Add(newProduct);
        await Task.Delay(200);
        return newProduct;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        await Task.Delay(200);
        return _products;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        await Task.Delay(200);
        return _products.FirstOrDefault(p => p.Id == id);
    }

    public async Task<Product?> UpdateAsync(int id, UpdateProductDto updateProductDto)
    {
        await Task.Delay(200);
        var index = _products.FindIndex(p => p.Id == id);
        if (index == -1) return null;
        var product = _products[index];
        updateProductDto.Adapt(product);
        return product;
    }

    public async Task<bool> Delete(int id)
    {
        var product = await GetByIdAsync(id);
        if (product is null) return false;

        _products.Remove(product);
        return true;
    }
}