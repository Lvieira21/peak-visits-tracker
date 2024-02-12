namespace Tracker.Pixel.Service.Core;

using Confluent.Kafka;
using Tracker.Pixel.Service.Service;

internal static class ServiceConfigurationsExtension
{
    internal static void AddTrackingServiceConfigurations(this IServiceCollection services,
        IConfiguration configuration)
    {
        var kafkaSettings = configuration.GetSection("Kafka").Get<KafkaSettings>();

        services.AddSingleton(kafkaSettings);

        services.AddSingleton(
            new ProducerBuilder<string, string>(
                new ProducerConfig { BootstrapServers = kafkaSettings.BootstrapServer })
                .Build());

        services.AddSingleton<IMessageProducerService, MessageProducerService>();
    }
}