using ProductCatalogue.Api.Mappings;
using ProductCatalogue.Api.Models;
using ProductCatalogue.Api.Services;

var builder = WebApplication.CreateBuilder(args);

MappingConfig.RegisterMappings();

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddSingleton<List<Product>>(
[
    new (){
        Id = Guid.NewGuid(),
        Name = "Classic Oxford Shirt",
        ProductCode = "SHT-001",
        Description = "A timeless Oxford shirt crafted from premium cotton.",
        Brand = "Jack & Jones",
        Category = "Shirts",
        TargetMarket = "Men",
        Season = "SS25"
       },
    new (){
        Id = Guid.NewGuid(),
        Name = "Slim Fit Chinos",
        ProductCode = "TRS-002",
        Description = "Modern slim fit chinos with a comfortable stretch fabric.",
        Brand = "Selected Homme",
        Category = "Trousers",
        TargetMarket = "Men",
        Season = "SS25"
    },
    new (){
        Id = Guid.NewGuid(),
        Name = "Leather Sneakers",
        ProductCode = "SNK-004",
        Description = "Clean leather sneakers with a minimalist silhouette.",
        Brand = "Pieces",
        Category = "Footwear",
        TargetMarket = "Unisex",
        Season = "SS25"
    },
    new (){
        Id = Guid.NewGuid(),
        Name = "Summer Floral Dress",
        ProductCode = "DRS-005",
        Description = "Light floral dress perfect for warm weather occasions.",
        Brand = "Only",
        Category = "Dresses",
        TargetMarket = "Women",
        Season = "SS25"
    },
    new (){
        Id = Guid.NewGuid(),
        Name = "Cargo Joggers",
        ProductCode = "JGR-006",
        Description = "Relaxed cargo joggers with multiple utility pockets.",
        Brand = "Jack & Jones",
        Category = "Trousers",
        TargetMarket = "Men",
        Season = "AW24",
    }
]);

builder.Services.AddSingleton<IProductService, ProductService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
