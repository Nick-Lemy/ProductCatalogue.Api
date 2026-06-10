using Mapster;
using ProductCatalogue.Api.DTOs;
using ProductCatalogue.Api.Models;

namespace ProductCatalogue.Api.Mappings;

public static class MappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<CreateProductDto, Product>.NewConfig()
            .Ignore(dest => dest.Id).IgnoreNullValues(true);
    }
}