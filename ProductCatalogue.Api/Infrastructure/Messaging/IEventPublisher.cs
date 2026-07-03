using ProductCatalogue.Contracts;

namespace ProductCatalogue.Api.Infrastructure.Messaging;

public interface IEventPublisher
{
    Task PublishAsync<TPayload>(string topic, string key, EventEnvelope<TPayload> envelope, CancellationToken cancellationToken = default);
}
