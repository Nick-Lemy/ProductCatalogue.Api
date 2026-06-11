using Microsoft.EntityFrameworkCore;
using ProductCatalogue.Api.Data;
using ProductCatalogue.Api.Exceptions;
using ProductCatalogue.Api.Mappings;
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

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")
    )
);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IVariantService, VariantService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

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
