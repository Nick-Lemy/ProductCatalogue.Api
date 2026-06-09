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
        Product newProduct = new() { Id = _products.Count + 1, Name = createProductDto.Name, Price = createProductDto.Price, Description = createProductDto.Description };

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

        product.Name = updateProductDto.Name ?? product.Name;
        product.Price = updateProductDto.Price ?? product.Price;
        product.Description = updateProductDto.Description ?? product.Description;

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