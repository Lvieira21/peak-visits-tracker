namespace Tracker.Pixel.Service.Models;

public record TrackingInfo(DateTime Date, string IpAddress, string? Referer, string? UserAgent);