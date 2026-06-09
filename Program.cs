using ProductCatalogue.Api.Models;
using ProductCatalogue.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<List<Product>>(
[
    new() { Id = 1, Name = "Laptop", Description = "A high-performance laptop suitable for gaming and work." },
    new() { Id = 2, Name = "Smartphone", Description = "A sleek smartphone with a powerful camera and long battery life." },
    new() { Id = 3, Name = "Headphones", Description = "Noise-cancelling headphones with superior sound quality." }
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

app.UseAuthorization();

app.MapControllers();

app.Run();
