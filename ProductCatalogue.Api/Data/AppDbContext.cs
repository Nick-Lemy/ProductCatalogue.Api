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

        Product[] products = [
            new()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
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
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
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
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
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
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
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
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
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
            new() { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), ProductId = products[0].Id, Name = "White / Small", VariantCode = "SHT-001-WHT-S", Colour = "White", Size = "S", Material = "100% Cotton", Barcode = "5901234123457", CreatedAt = seededAt, UpdatedAt = seededAt },
            new() { Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), ProductId = products[0].Id, Name = "White / Medium", VariantCode = "SHT-001-WHT-M", Colour = "White", Size = "M", Material = "100% Cotton", Barcode = "5901234123458", CreatedAt = seededAt, UpdatedAt = seededAt },
            new() { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), ProductId = products[0].Id, Name = "White / Large", VariantCode = "SHT-001-WHT-L", Colour = "White", Size = "L", Material = "100% Cotton", Barcode = "5901234123459", CreatedAt = seededAt, UpdatedAt = seededAt },
            new() { Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), ProductId = products[0].Id, Name = "Navy / Small", VariantCode = "SHT-001-NVY-S", Colour = "Navy", Size = "S", Material = "100% Cotton", CreatedAt = seededAt, UpdatedAt = seededAt },
            new() { Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), ProductId = products[0].Id, Name = "Navy / Medium", VariantCode = "SHT-001-NVY-M", Colour = "Navy", Size = "M", Material = "100% Cotton", CreatedAt = seededAt, UpdatedAt = seededAt },
            new() { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), ProductId = products[0].Id, Name = "Navy / Large", VariantCode = "SHT-001-NVY-L", Colour = "Navy", Size = "L", Material = "100% Cotton", CreatedAt = seededAt, UpdatedAt = seededAt },

            // Slim Fit Chinos variants
            new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111112"), ProductId = products[1].Id, Name = "Beige / 30", VariantCode = "TRS-002-BGE-30", Colour = "Beige", Size = "30", Material = "98% Cotton 2% Elastane", Barcode = "5901234123460", CreatedAt = seededAt, UpdatedAt = seededAt },
            new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111113"), ProductId = products[1].Id, Name = "Beige / 32", VariantCode = "TRS-002-BGE-32", Colour = "Beige", Size = "32", Material = "98% Cotton 2% Elastane", Barcode = "5901234123461", CreatedAt = seededAt, UpdatedAt = seededAt },
            new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111114"), ProductId = products[1].Id, Name = "Khaki / 30", VariantCode = "TRS-002-KHK-30", Colour = "Khaki", Size = "30", Material = "98% Cotton 2% Elastane", CreatedAt = seededAt, UpdatedAt = seededAt },
            new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111115"), ProductId = products[1].Id, Name = "Khaki / 32", VariantCode = "TRS-002-KHK-32", Colour = "Khaki", Size = "32", Material = "98% Cotton 2% Elastane", CreatedAt = seededAt, UpdatedAt = seededAt },

            new() { Id = Guid.Parse("22222222-2222-2222-2222-222222222223"), ProductId = products[2].Id, Name = "White / 40", VariantCode = "SNK-004-WHT-40", Colour = "White", Size = "40", Material = "Leather", Barcode = "5901234123462", CreatedAt = seededAt, UpdatedAt = seededAt },
            new() { Id = Guid.Parse("22222222-2222-2222-2222-222222222224"), ProductId = products[2].Id, Name = "White / 41", VariantCode = "SNK-004-WHT-41", Colour = "White", Size = "41", Material = "Leather", Barcode = "5901234123463", CreatedAt = seededAt, UpdatedAt = seededAt },
            new() { Id = Guid.Parse("22222222-2222-2222-2222-222222222225"), ProductId = products[2].Id, Name = "Black / 40", VariantCode = "SNK-004-BLK-40", Colour = "Black", Size = "40", Material = "Leather", CreatedAt = seededAt, UpdatedAt = seededAt },
            new() { Id = Guid.Parse("22222222-2222-2222-2222-222222222226"), ProductId = products[2].Id, Name = "Black / 41", VariantCode = "SNK-004-BLK-41", Colour = "Black", Size = "41", Material = "Leather", CreatedAt = seededAt, UpdatedAt = seededAt },

            new() { Id = Guid.Parse("33333333-3333-3333-3333-333333333334"), ProductId = products[3].Id, Name = "Pink / XS", VariantCode = "DRS-005-PNK-XS", Colour = "Pink", Size = "XS", Material = "100% Viscose", Barcode = "5901234123464", CreatedAt = seededAt, UpdatedAt = seededAt },
            new() { Id = Guid.Parse("33333333-3333-3333-3333-333333333335"), ProductId = products[3].Id, Name = "Pink / S", VariantCode = "DRS-005-PNK-S", Colour = "Pink", Size = "S", Material = "100% Viscose", Barcode = "5901234123465", CreatedAt = seededAt, UpdatedAt = seededAt },
            new() { Id = Guid.Parse("33333333-3333-3333-3333-333333333336"), ProductId = products[3].Id, Name = "Blue / XS", VariantCode = "DRS-005-BLU-XS", Colour = "Blue", Size = "XS", Material = "100% Viscose", CreatedAt = seededAt, UpdatedAt = seededAt },
            new() { Id = Guid.Parse("33333333-3333-3333-3333-333333333337"), ProductId = products[3].Id, Name = "Blue / S", VariantCode = "DRS-005-BLU-S", Colour = "Blue", Size = "S", Material = "100% Viscose", CreatedAt = seededAt, UpdatedAt = seededAt },

            new() { Id = Guid.Parse("44444444-4444-4444-4444-444444444445"), ProductId = products[4].Id, Name = "Black / S", VariantCode = "JGR-006-BLK-S", Colour = "Black", Size = "S", Material = "100% Cotton", Barcode = "5901234123466", CreatedAt = seededAt, UpdatedAt = seededAt },
            new() { Id = Guid.Parse("44444444-4444-4444-4444-444444444446"), ProductId = products[4].Id, Name = "Black / M", VariantCode = "JGR-006-BLK-M", Colour = "Black", Size = "M", Material = "100% Cotton", Barcode = "5901234123467", CreatedAt = seededAt, UpdatedAt = seededAt },
            new() { Id = Guid.Parse("44444444-4444-4444-4444-444444444447"), ProductId = products[4].Id, Name = "Olive / S", VariantCode = "JGR-006-OLV-S", Colour = "Olive", Size = "S", Material = "100% Cotton", CreatedAt = seededAt, UpdatedAt = seededAt },
            new() { Id = Guid.Parse("44444444-4444-4444-4444-444444444448"), ProductId = products[4].Id, Name = "Olive / M", VariantCode = "JGR-006-OLV-M", Colour = "Olive", Size = "M", Material = "100% Cotton", CreatedAt = seededAt, UpdatedAt = seededAt },
];
        modelBuilder.Entity<Product>().HasData(products);
        modelBuilder.Entity<Variant>().HasData(variants);
    }
}