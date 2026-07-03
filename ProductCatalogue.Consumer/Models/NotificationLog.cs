namespace ProductCatalogue.Consumer.Models;

public class NotificationLog
{
    public Guid Id { get; set; }
    public required string EventType { get; set; }
    public Guid AssetId { get; set; }
    public Guid ProductId { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}
