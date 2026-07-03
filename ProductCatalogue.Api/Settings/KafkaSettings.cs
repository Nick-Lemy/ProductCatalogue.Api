namespace ProductCatalogue.Api.Settings;

public class KafkaSettings
{
    public required string BootstrapServers { get; set; }
    public required string AssetEventsTopic { get; set; }
    public required string ProductEventsTopic { get; set; }
}
