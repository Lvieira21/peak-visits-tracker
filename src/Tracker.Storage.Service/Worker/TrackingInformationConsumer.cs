using System.Globalization;
using System.Text;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Tracker.Storage.Service.Core;
using Tracker.Storage.Service.Events;

namespace Tracker.Storage.Service.Worker;

using Confluent.Kafka;

public class TrackingInformationConsumer : BackgroundService
{
    private readonly KafkaSettings kafkaSettings;
    private readonly string fileLocation;
    private readonly ConsumerConfig consumerConfig;
    private readonly ILogger<TrackingInformationConsumer> log;

    public TrackingInformationConsumer(
        KafkaSettings kafkaSettings,
        ILogger<TrackingInformationConsumer> log,
        IConfiguration configuration)
    {
        this.kafkaSettings = kafkaSettings;
        this.log = log;
        this.fileLocation = configuration["FileLocation"];

        this.consumerConfig = new ConsumerConfig 
        {
            BootstrapServers = kafkaSettings.BootstrapServer,
            GroupId = kafkaSettings.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true,
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        {
            consumer.Subscribe(kafkaSettings.ConsumerTopic);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    this.log.LogInformation("Trying to consume from topic: {}", kafkaSettings.ConsumerTopic);
                    
                    var result = consumer.Consume(stoppingToken);

                    this.log.LogInformation("Message consumed properly: {}", result.Message);
                    
                    await HandleMessageAsync(result.Message.Value, stoppingToken);
                }
                catch (ConsumeException e)
                {
                    this.log.LogError(e, "Kafka Topic: {} was not found", kafkaSettings.ConsumerTopic);
                }
            }
        }
    }

    private async Task HandleMessageAsync(string message, CancellationToken cancellationToken)
    {
        var trackingInfo = JsonConvert.DeserializeObject<TrackingInfoEvent>(message);

        if (trackingInfo is null || string.IsNullOrEmpty(trackingInfo.IpAddress))
        {
            return;
        }
        
        this.log.LogInformation("Logging tracking information {trackingInfo} in folder {fileLocation}",
            trackingInfo,
            this.fileLocation);
        
        var logging =
            $"{(trackingInfo.Date.ToString("o", CultureInfo.InvariantCulture))}" +
            $"|{trackingInfo.Referer ?? "null"}" +
            $"|{trackingInfo.UserAgent ?? "null"}" +
            $"|{trackingInfo.IpAddress}\n";

        await File.AppendAllTextAsync(fileLocation, logging, cancellationToken);
    }
}