using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using ProductCatalogue.Api.Settings;
using ProductCatalogue.Contracts;

namespace ProductCatalogue.Api.Infrastructure.Messaging;

public sealed class KafkaEventPublisher : IEventPublisher, IDisposable
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaEventPublisher> _logger;

    public KafkaEventPublisher(IOptions<KafkaSettings> settings, ILogger<KafkaEventPublisher> logger)
    {
        _logger = logger;

        var config = new ProducerConfig
        {
            BootstrapServers = settings.Value.BootstrapServers,
            Acks = Acks.All,
            EnableIdempotence = true,
            MessageTimeoutMs = 5000
        };

        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public async Task PublishAsync<TPayload>(string topic, string key, EventEnvelope<TPayload> envelope, CancellationToken cancellationToken = default)
    {
        var message = new Message<string, string>
        {
            Key = key,
            Value = JsonSerializer.Serialize(envelope, EventSerialization.Options)
        };

        try
        {
            var result = await _producer.ProduceAsync(topic, message, cancellationToken);
            _logger.LogInformation(
                "[Kafka] Published {EventType} ({EventId}) to {TopicPartitionOffset}",
                envelope.EventType, envelope.EventId, result.TopicPartitionOffset);
        }
        catch (ProduceException<string, string> ex)
        {
            _logger.LogError(ex,
                "[Kafka] Failed to publish {EventType} ({EventId}) to {Topic}: {Reason}",
                envelope.EventType, envelope.EventId, topic, ex.Error.Reason);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "[Kafka] Unexpected error publishing {EventType} ({EventId}) to {Topic}",
                envelope.EventType, envelope.EventId, topic);
        }
    }

    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(5));
        _producer.Dispose();
    }
}
