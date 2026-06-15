using System.Text.Json.Serialization;

namespace ProductCatalogue.Api.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AssetStatus
{
    PENDING_REVIEW,
    APPROVED,
    REJECTED
}