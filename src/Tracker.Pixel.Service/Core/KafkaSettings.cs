namespace Tracker.Pixel.Service.Core;

public class KafkaSettings
{
    public string BootstrapServer { get; set; }
    
    public string ProduceTopic { get; set; }
}