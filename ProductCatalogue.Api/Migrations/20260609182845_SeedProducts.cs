using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductCatalogue.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Brand", "Category", "CreatedAt", "Description", "Name", "ProductCode", "Readiness", "Season", "Status", "TargetMarket", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), "Jack & Jones", "Shirts", new DateTimeOffset(new DateTime(2026, 6, 9, 18, 28, 44, 577, DateTimeKind.Unspecified).AddTicks(5145), new TimeSpan(0, 0, 0, 0, 0)), "A timeless Oxford shirt crafted from premium cotton.", "Classic Oxford Shirt", "SHT-001", 1, "SS25", 0, "Men", new DateTimeOffset(new DateTime(2026, 6, 9, 18, 28, 44, 577, DateTimeKind.Unspecified).AddTicks(5146), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), "Selected Homme", "Trousers", new DateTimeOffset(new DateTime(2026, 6, 9, 18, 28, 44, 577, DateTimeKind.Unspecified).AddTicks(9694), new TimeSpan(0, 0, 0, 0, 0)), "Modern slim fit chinos with a comfortable stretch fabric.", "Slim Fit Chinos", "TRS-002", 1, "SS25", 0, "Men", new DateTimeOffset(new DateTime(2026, 6, 9, 18, 28, 44, 577, DateTimeKind.Unspecified).AddTicks(9695), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), "Pieces", "Footwear", new DateTimeOffset(new DateTime(2026, 6, 9, 18, 28, 44, 577, DateTimeKind.Unspecified).AddTicks(9733), new TimeSpan(0, 0, 0, 0, 0)), "Clean leather sneakers with a minimalist silhouette.", "Leather Sneakers", "SNK-004", 1, "SS25", 0, "Unisex", new DateTimeOffset(new DateTime(2026, 6, 9, 18, 28, 44, 577, DateTimeKind.Unspecified).AddTicks(9733), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("d4e5f6a7-b8c9-0123-defa-234567890123"), "Only", "Dresses", new DateTimeOffset(new DateTime(2026, 6, 9, 18, 28, 44, 577, DateTimeKind.Unspecified).AddTicks(9752), new TimeSpan(0, 0, 0, 0, 0)), "Light floral dress perfect for warm weather occasions.", "Summer Floral Dress", "DRS-005", 1, "SS25", 0, "Women", new DateTimeOffset(new DateTime(2026, 6, 9, 18, 28, 44, 577, DateTimeKind.Unspecified).AddTicks(9752), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e5f6a7b8-c9d0-1234-efab-345678901234"), "Jack & Jones", "Trousers", new DateTimeOffset(new DateTime(2026, 6, 9, 18, 28, 44, 577, DateTimeKind.Unspecified).AddTicks(9771), new TimeSpan(0, 0, 0, 0, 0)), "Relaxed cargo joggers with multiple utility pockets.", "Cargo Joggers", "JGR-006", 1, "AW24", 0, "Men", new DateTimeOffset(new DateTime(2026, 6, 9, 18, 28, 44, 577, DateTimeKind.Unspecified).AddTicks(9771), new TimeSpan(0, 0, 0, 0, 0)) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-0123-defa-234567890123"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e5f6a7b8-c9d0-1234-efab-345678901234"));
        }
    }
}
