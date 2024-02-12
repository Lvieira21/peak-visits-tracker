namespace Tracker.Pixel.Service.Service;

using Tracker.Pixel.Service.Models;

public interface IMessageProducerService
{
    Task ProduceMessageAsync(TrackingInfo trackingInfo);
}