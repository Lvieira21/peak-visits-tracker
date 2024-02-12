using Tracker.Storage.Service.Core;
using Tracker.Storage.Service.Worker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var kafkaSettings = hostContext.Configuration.GetSection("Kafka").Get<KafkaSettings>();

        services.AddSingleton(kafkaSettings);
        
        services.AddHostedService<TrackingInformationConsumer>();
    })
    .Build();

await host.RunAsync();