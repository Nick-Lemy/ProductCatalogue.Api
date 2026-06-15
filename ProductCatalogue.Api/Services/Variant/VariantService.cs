using Mapster;
using Microsoft.EntityFrameworkCore;
using ProductCatalogue.Api.Data;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Exceptions;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public class VariantService(
    AppDbContext context,
    ILogger<VariantService> logger) : IVariantService
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<VariantService> _logger = logger;
    public async Task<VariantReponseDto> CreateAsync(CreateVariantDto createVariantDto)
    {
        _logger.LogInformation("[Variant] Creating variant with name {Name} for product {ProductId}", createVariantDto.Name, createVariantDto.ProductId);

        Product? product = await _context.Products.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == createVariantDto.ProductId);
        if (product is null)
            throw new NotFoundException($"Product with id {createVariantDto.ProductId} not found");

        Variant? variantWithSameCode = await _context.Variants.AsNoTracking()
            .FirstOrDefaultAsync(v => v.VariantCode == createVariantDto.VariantCode);
        if (variantWithSameCode is not null)
            throw new ConflictException($"Variant with code {createVariantDto.VariantCode} already exists");

        if (createVariantDto.Barcode is not null)
        {
            Variant? variantWithSameBarcode = await _context.Variants.AsNoTracking()
             .FirstOrDefaultAsync(v => v.Barcode != null && v.Barcode == createVariantDto.Barcode);
            if (variantWithSameBarcode is not null)
                throw new ConflictException($"Variant with barcode {createVariantDto.Barcode} already exists");
        }

        Variant newVariant = createVariantDto.Adapt<Variant>();
        _context.Variants.Add(newVariant);
        await _context.SaveChangesAsync();
        _logger.LogInformation("[Variant] Variant {VariantId} created successfully for product {ProductId}", newVariant.Id, createVariantDto.ProductId);
        return newVariant.Adapt<VariantReponseDto>();
    }


    public async Task<List<VariantReponseDto>> GetAllAsync(VariantQueryDto query)
    {
        _logger.LogInformation("[Variant] Fetching variants with query {@Query}", query);
        var variants = await _context.Variants.AsNoTracking()
            .Where(v => query.ProductId == null || v.ProductId == query.ProductId)
            .Where(v => query.Name == null || EF.Functions.ILike(v.Name, $"%{query.Name}%"))
            .Where(v => query.VariantCode == null || v.VariantCode == query.VariantCode)
            .Where(v => query.Colour == null || v.Colour == query.Colour)
            .Where(v => query.Size == null || v.Size == query.Size)
            .Where(v => query.Material == null || v.Material == query.Material)
            .Where(v => query.Barcode == null || v.Barcode == query.Barcode)
            .ToListAsync();

        _logger.LogInformation("[Variant] Fetched {Count} variants", variants.Count);
        return variants.Adapt<List<VariantReponseDto>>();
    }

    public async Task<VariantReponseDto> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("[Variant] Fetching variant with id {Id}", id);
        Variant? variant = await _context.Variants.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
        if (variant is null)
            throw new NotFoundException($"Variant with id {id} not found");

        _logger.LogInformation("[Variant] Variant fetched successfully with id {Id}", id);
        return variant.Adapt<VariantReponseDto>();
    }

    public async Task<VariantReponseDto> UpdateAsync(Guid id, UpdateVariantDto updateVariantDto)
    {
        _logger.LogInformation("[Variant] Updating variant with id {Id}", id);

        if (updateVariantDto.ProductId != null)
        {
            Product? product = await _context.Products.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == updateVariantDto.ProductId);
            if (product is null)
                throw new NotFoundException($"Product with id {updateVariantDto.ProductId} not found");
        }

        Variant? variant = await _context.Variants.FindAsync(id);
        if (variant is null)
            throw new NotFoundException($"Variant with id {id} not found");

        if (updateVariantDto.VariantCode is not null && variant.VariantCode != updateVariantDto.VariantCode)
        {
            Variant? variantWithSameCode = await _context.Variants.AsNoTracking()
             .FirstOrDefaultAsync(v => v.VariantCode == updateVariantDto.VariantCode && v.Id != id);
            if (variantWithSameCode is not null)
                throw new ConflictException($"Variant with code {updateVariantDto.VariantCode} already exists");
        }

        if (updateVariantDto.Barcode is not null && variant.Barcode != updateVariantDto.Barcode)
        {
            Variant? variantWithSameBarcode = await _context.Variants.AsNoTracking()
             .FirstOrDefaultAsync(v => v.Barcode != null && v.Barcode == updateVariantDto.Barcode && v.Id != id);
            if (variantWithSameBarcode is not null)
                throw new ConflictException($"Variant with barcode {updateVariantDto.Barcode} already exists");
        }

        updateVariantDto.Adapt(variant);
        variant.UpdatedAt = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync();
        _logger.LogInformation("[Variant] Updated variant with id {Id}", id);
        return variant.Adapt<VariantReponseDto>();
    }
    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("[Variant] Deleting variant with id {Id}", id);
        Variant? variant = await _context.Variants.FindAsync(id);
        if (variant is null)
            throw new NotFoundException($"Variant with id {id} not found");

        _context.Variants.Remove(variant);
        await _context.SaveChangesAsync();
        _logger.LogInformation("[Variant] Deleted variant with id {Id}", id);
    }
}