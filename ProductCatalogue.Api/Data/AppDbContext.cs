using Microsoft.EntityFrameworkCore;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Variant> Variants { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        DateTimeOffset seededAt = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

        Product[] products = [ new()
            {
                Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                Name = "Classic Oxford Shirt",
                ProductCode = "SHT-001",
                Description = "A timeless Oxford shirt crafted from premium cotton.",
                Brand = "Jack & Jones",
                Category = "Shirts",
                TargetMarket = "Men",
                Season = "SS25",
                CreatedAt = seededAt,
                UpdatedAt = seededAt
            },
            new()
            {
                Id = Guid.Parse("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                Name = "Slim Fit Chinos",
                ProductCode = "TRS-002",
                Description = "Modern slim fit chinos with a comfortable stretch fabric.",
                Brand = "Selected Homme",
                Category = "Trousers",
                TargetMarket = "Men",
                Season = "SS25",
                CreatedAt = seededAt,
                UpdatedAt = seededAt
            },
            new()
            {
                Id = Guid.Parse("c3d4e5f6-a7b8-9012-cdef-123456789012"),
                Name = "Leather Sneakers",
                ProductCode = "SNK-004",
                Description = "Clean leather sneakers with a minimalist silhouette.",
                Brand = "Pieces",
                Category = "Footwear",
                TargetMarket = "Unisex",
                Season = "SS25",
                CreatedAt = seededAt,
                UpdatedAt = seededAt
            },
            new()
            {
                Id = Guid.Parse("d4e5f6a7-b8c9-0123-defa-234567890123"),
                Name = "Summer Floral Dress",
                ProductCode = "DRS-005",
                Description = "Light floral dress perfect for warm weather occasions.",
                Brand = "Only",
                Category = "Dresses",
                TargetMarket = "Women",
                Season = "SS25",
                CreatedAt = seededAt,
                UpdatedAt = seededAt
            },
            new()
            {
                Id = Guid.Parse("e5f6a7b8-c9d0-1234-efab-345678901234"),
                Name = "Cargo Joggers",
                ProductCode = "JGR-006",
                Description = "Relaxed cargo joggers with multiple utility pockets.",
                Brand = "Jack & Jones",
                Category = "Trousers",
                TargetMarket = "Men",
                Season = "AW24",
                CreatedAt = seededAt,
                UpdatedAt = seededAt
            }
        ];
        Variant[] variants = [
            new(){
                Id = Guid.Parse("v1"),
                ProductId  = products[0].Id,
                Name = "White / Small",
                VariantCode = "SHT-001-WHT-S",
                Colour = "White",
                Size = "S",
                Material = "100% Cotton",
                Barcode = "5901234123457",
                CreatedAt = seededAt,
                UpdatedAt = seededAt,
            },
            new(){
                Id = Guid.Parse("v2"),
                ProductId  = products[0].Id,
                Name = "White / Medium",
                VariantCode = "SHT-001-WHT-M",
                Colour = "White",
                Size = "M",
                Material = "100% Cotton",
                Barcode = "5901234123458",
                CreatedAt = seededAt,
                UpdatedAt = seededAt,
            },
            new(){
                Id = Guid.Parse("v3"),
                ProductId  = products[0].Id,
                Name = "Navy / Medium",
                VariantCode = "SHT-001-NVY-M",
                Colour = "Navy",
                Size = "M",
                Material = "100% Cotton",
                CreatedAt = seededAt,
                UpdatedAt = seededAt,
            },

        ];

        modelBuilder.Entity<Product>().HasData(products);
        modelBuilder.Entity<Variant>().HasData(variants);
    }
}