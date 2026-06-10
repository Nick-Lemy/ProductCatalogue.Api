using System.Text.Json.Serialization;

namespace ProductCatalogue.Api.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProductStatus
{
    DRAFT,
    IN_REVIEW,
    PUBLISHED,
    ARCHIVED,
}
