using Microsoft.EntityFrameworkCore;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
}