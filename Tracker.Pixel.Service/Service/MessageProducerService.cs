namespace Tracker.Pixel.Service.Service;

using Confluent.Kafka;
using Newtonsoft.Json;
using Tracker.Pixel.Service.Core;
using Tracker.Pixel.Service.Models;

public class MessageProducerService : IMessageProducerService
{
    private readonly IProducer<string, string> producer;
    private readonly KafkaSettings kafkaSettings;

    public MessageProducerService(IProducer<string, string> producer, KafkaSettings kafkaSettings)
    {
        this.producer = producer;
        this.kafkaSettings = kafkaSettings;
    }
    
    public async Task ProduceMessageAsync(TrackingInfo trackingInfo)
    {
        var serializedTrackingInfo = JsonConvert.SerializeObject(trackingInfo);
        
        await producer.ProduceAsync(this.kafkaSettings.ProduceTopic, new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = serializedTrackingInfo
        });
    }
}