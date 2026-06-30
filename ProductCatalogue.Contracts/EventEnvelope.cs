namespace ProductCatalogue.Contracts;

public record EventEnvelope<TPayload>
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public required string EventType { get; init; }
    public DateTimeOffset OccurredAt { get; init; } = DateTimeOffset.UtcNow;
    public int Version { get; init; } = 1;
    public required TPayload Payload { get; init; }
}
