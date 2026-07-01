namespace ProductCatalogue.Consumer.Settings;

public class KafkaConsumerSettings
{
    public required string BootstrapServers { get; set; }
    public required string AssetEventsTopic { get; set; }
    public required string ConsumerGroup { get; set; }
}
