namespace Tracker.Pixel.Service.Service;

using Confluent.Kafka;
using Newtonsoft.Json;
using Tracker.Pixel.Service.Core;
using Tracker.Pixel.Service.Models;

public class MessageProducerService : IMessageProducerService
{
    private readonly IProducer<string, string> producer;
    private readonly KafkaSettings kafkaSettings;
    private readonly ILogger<MessageProducerService> logger;

    public MessageProducerService(
        IProducer<string, string> producer,
        KafkaSettings kafkaSettings,
        ILogger<MessageProducerService> logger)
    {
        this.producer = producer;
        this.kafkaSettings = kafkaSettings;
        this.logger = logger;
    }
    
    public async Task ProduceMessageAsync(TrackingInfo trackingInfo)
    {
        logger.LogInformation("TrackingInfo Received: {}", trackingInfo);
        
        var serializedTrackingInfo = JsonConvert.SerializeObject(trackingInfo);
        
        logger.LogInformation("Producing message with tracking information to kafka topic {}",
            this.kafkaSettings.ProduceTopic);
        
        await producer.ProduceAsync(this.kafkaSettings.ProduceTopic, new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = serializedTrackingInfo
        });
    }
}