# Product Catalogue API

Backend for the product catalogue and asset management platform. It manages products, variants, and assets. It also publishes Kafka events when key things happen to products and assets. A separate consumer reacts to asset events.

## Project structure

```
ProductCatalogue.Api/         API and event publisher
ProductCatalogue.Consumer/    Console app that reads asset events
ProductCatalogue.Contracts/   Shared event classes (envelope and payloads)
ProductCatalogue.Tests/       Unit tests
docker-compose.yml            Local PostgreSQL and Kafka
.env.example                  Template for Docker values
```

## Requirements

- .NET 10 SDK
- Docker

## Configuration

Config is stored in user-secrets, not in appsettings.json.

API secrets:

```bash
cd ProductCatalogue.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=3003;Database=product_catalogue_db;Username=<user>;Password=<password>"
dotnet user-secrets set "Kafka:BootstrapServers" "localhost:9092"
dotnet user-secrets set "Kafka:AssetEventsTopic" "catalogue.asset-events"
dotnet user-secrets set "Kafka:ProductEventsTopic" "catalogue.product-events"
```

Consumer secrets:

```bash
cd ProductCatalogue.Consumer
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=3003;Database=product_catalogue_db;Username=<user>;Password=<password>"
dotnet user-secrets set "Kafka:BootstrapServers" "localhost:9092"
dotnet user-secrets set "Kafka:AssetEventsTopic" "catalogue.asset-events"
dotnet user-secrets set "Kafka:ConsumerGroup" "notification-log-consumer"
```

## Run locally

Run all commands from the repo root.

Step 1. Start PostgreSQL and Kafka. This also creates the two topics.

```bash
cp .env.example .env
docker compose up -d
```

Step 2. Create the database schema.

```bash
dotnet ef database update --project ProductCatalogue.Api
dotnet ef database update --project ProductCatalogue.Consumer
```

Step 3. Start the API.

```bash
dotnet run --project ProductCatalogue.Api
```

API runs at http://localhost:5093. Swagger UI is at http://localhost:5093/swagger.

Step 4. Start the consumer in a second terminal.

```bash
dotnet run --project ProductCatalogue.Consumer
```

## Verify that messages are flowing

1. Open Swagger and log in to get a token.
2. Approve or reject an asset, or submit or publish a product.
3. The API terminal logs a line like `[Kafka] Published AssetApproved`.
4. The consumer terminal logs a line like `[Consumer] Logged AssetApproved`.
5. A new row appears in the `NotificationLogs` table.

You can also read a topic directly:

```bash
docker compose exec kafka /opt/kafka/bin/kafka-console-consumer.sh --bootstrap-server kafka:29092 --topic catalogue.asset-events --from-beginning
```

## Topics

There are two topics:

- `catalogue.asset-events` for asset events
- `catalogue.product-events` for product events

Each topic has 3 partitions. We chose 3 so up to 3 consumers in a group can work at the same time. The product ID is used as the message key, so all events for one product go to the same partition and keep their order. One partition would block parallel work. Many more would add no value at this size.

## Events

The API publishes an event when:

- an asset is uploaded
- an asset is approved
- an asset is rejected
- a product is submitted for review
- a product is published

Every event is a JSON message with this envelope:

- `eventId`: a GUID, unique per event
- `eventType`: for example `AssetApproved`
- `occurredAt`: UTC time in ISO 8601
- `version`: an integer, starts at 1
- `payload`: the event data

The payload carries enough data to act on without calling the API back. The message key is the product ID.

## Producer behaviour

- Publishing sits behind the `IEventPublisher` abstraction. Services depend on it, not on Kafka.
- Kafka settings come from config. Nothing is hard-coded.
- The producer waits for the broker to confirm each message before it counts as sent.
- Events are published only after the database change is committed. A rolled back change is never announced.
- If Kafka is down, the API does not hang and does not return 500. The database change still succeeds. The publish error is logged and the request returns success.

## Consumer behaviour

- It is a console app that runs a `BackgroundService`.
- It joins a named group and subscribes to the asset events topic.
- On `AssetApproved` or `AssetRejected` it writes a row to `NotificationLog` with the event type, asset ID, product ID, and timestamp.
- It commits offsets after each message and closes cleanly on shutdown.
- A malformed message is logged and skipped. It does not crash the consumer.

## Run tests

```bash
dotnet test
```
