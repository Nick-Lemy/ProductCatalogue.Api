using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductCatalogue.Consumer.Data;
using ProductCatalogue.Consumer.Models;
using ProductCatalogue.Consumer.Settings;
using ProductCatalogue.Contracts;

namespace ProductCatalogue.Consumer;

public class NotificationConsumer(
    IServiceScopeFactory scopeFactory,
    IOptions<KafkaConsumerSettings> settings,
    ILogger<NotificationConsumer> logger) : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly KafkaConsumerSettings _settings = settings.Value;
    private readonly ILogger<NotificationConsumer> _logger = logger;

    private record AssetEventData(Guid AssetId, Guid ProductId);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        var config = new ConsumerConfig
        {
            BootstrapServers = _settings.BootstrapServers,
            GroupId = _settings.ConsumerGroup,
            EnableAutoCommit = false,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();
        consumer.Subscribe(_settings.AssetEventsTopic);
        _logger.LogInformation("[Consumer] Subscribed to {Topic} as group {Group}", _settings.AssetEventsTopic, _settings.ConsumerGroup);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);
                    await HandleAsync(result.Message.Value, stoppingToken);
                    consumer.Commit(result);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "[Consumer] Consume error: {Reason}", ex.Error.Reason);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogError(ex, "[Consumer] Unexpected error while processing a message");
                }
            }
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            consumer.Close();
            _logger.LogInformation("[Consumer] Closed cleanly");
        }
    }

    private async Task HandleAsync(string rawMessage, CancellationToken cancellationToken)
    {
        EventEnvelope<JsonElement>? envelope;
        try
        {
            envelope = JsonSerializer.Deserialize<EventEnvelope<JsonElement>>(rawMessage, EventSerialization.Options);
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "[Consumer] Skipping malformed message: {Raw}", rawMessage);
            return;
        }

        if (envelope is null)
        {
            _logger.LogWarning("[Consumer] Skipping empty message");
            return;
        }

        if (envelope.EventType is not (EventTypes.AssetApproved or EventTypes.AssetRejected))
            return;

        AssetEventData? data;
        try
        {
            data = envelope.Payload.Deserialize<AssetEventData>(EventSerialization.Options);
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "[Consumer] Skipping message with malformed payload for {EventType}", envelope.EventType);
            return;
        }

        if (data is null)
            return;

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();

        db.NotificationLogs.Add(new NotificationLog
        {
            EventType = envelope.EventType,
            AssetId = data.AssetId,
            ProductId = data.ProductId,
            Timestamp = envelope.OccurredAt
        });
        await db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("[Consumer] Logged {EventType} for asset {AssetId}", envelope.EventType, data.AssetId);
    }
}
