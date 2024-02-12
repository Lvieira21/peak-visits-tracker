namespace Tracker.Storage.Service.Events;

public record TrackingInfoEvent(DateTime Date, string IpAddress, string? Referer, string? UserAgent);