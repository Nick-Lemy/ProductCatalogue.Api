using Mapster;
using Microsoft.EntityFrameworkCore;
using ProductCatalogue.Api.Data;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Services;

public class VariantService(AppDbContext context) : IVariantService
{
    private readonly AppDbContext _context = context;

    public async Task<VariantReponseDto> CreateAsync(CreateVariantDto createVariantDto)
    {
        Variant newVariant = createVariantDto.Adapt<Variant>();
        _context.Variants.Add(newVariant);
        await _context.SaveChangesAsync();
        return newVariant.Adapt<VariantReponseDto>();
    }


    public async Task<List<VariantReponseDto>> GetAllAsync(VariantQueryDto query)
    {
        var variants = await _context.Variants.AsNoTracking()
            .Where(v => query.ProductId == null || v.ProductId == query.ProductId)
            .Where(v => query.Name == null || v.Name.Contains(query.Name, StringComparison.CurrentCultureIgnoreCase))
            .Where(v => query.VariantCode == null || v.VariantCode == query.VariantCode)
            .Where(v => query.Colour == null || v.Colour == query.Colour)
            .Where(v => query.Size == null || v.Size == query.Size)
            .Where(v => query.Material == null || v.Material == query.Material)
            .Where(v => query.Barcode == null || v.Barcode == query.Barcode)
            .ToListAsync();

        return variants.Adapt<List<VariantReponseDto>>();
    }

    public async Task<VariantReponseDto?> GetByIdAsync(Guid id)
    {
        Variant? variant = await _context.Variants.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
        return variant?.Adapt<VariantReponseDto>();
    }

    public async Task<VariantReponseDto?> UpdateAsync(Guid id, UpdateVariantDto updateVariantDto)
    {
        Variant? variant = await _context.Variants.FindAsync(id);
        if (variant is null) return null;

        updateVariantDto.Adapt(variant);
        variant.UpdatedAt = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync();
        return variant.Adapt<VariantReponseDto>();
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        Variant? variant = await _context.Variants.FindAsync(id);
        if (variant is null) return false;

        _context.Variants.Remove(variant);
        await _context.SaveChangesAsync();
        return true;
    }
}