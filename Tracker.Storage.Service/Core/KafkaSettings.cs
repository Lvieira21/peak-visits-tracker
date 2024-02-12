namespace Tracker.Storage.Service.Core;

public class KafkaSettings
{
    public string BootstrapServer { get; set; }
    
    public string GroupId { get; set; }
    
    public string ConsumerTopic { get; set; }
}