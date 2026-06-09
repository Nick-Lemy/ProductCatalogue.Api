using System.Text.Json.Serialization;

namespace ProductCatalogue.Api.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProductReadiness
{
    Ready,
    NotReady
}