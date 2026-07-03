using Microsoft.AspNetCore.Mvc;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public interface IProductService
{ 
    public Task<Result<List<ProductResponseDto>>> GetAllAsync(ProductQueryDto query);
    public Task<Result<ProductResponseDto>> GetByIdAsync(Guid id);
    public Task<Result<ProductResponseDto>> CreateAsync(CreateProductDto createProductDto);
    public Task<Result<ProductResponseDto>> UpdateAsync(Guid id, UpdateProductDto updateProductDto);
    public Task<Result<ProductResponseDto>> ChangeStatusAsync(Guid id, ChangeStatusDto changeStatusDto);
    public Task<Result<ProductResponseDto>> SubmitForReviewAsync(Guid id);
    public Task<Result<ProductResponseDto>> PublishAsync(Guid id);
    public Task<Result> DeleteAsync(Guid id);
}