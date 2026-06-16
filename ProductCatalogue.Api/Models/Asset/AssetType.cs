using System.Text.Json.Serialization;

namespace ProductCatalogue.Api.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AssetType
{
    IMAGE,
    VIDEO,
    DOCUMENT
}