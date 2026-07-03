using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductCatalogue.Consumer;
using ProductCatalogue.Consumer.Data;
using ProductCatalogue.Consumer.Settings;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<KafkaConsumerSettings>(builder.Configuration.GetSection("Kafka"));

builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory_Consumer")));

builder.Services.AddHostedService<NotificationConsumer>();

var host = builder.Build();
host.Run();
