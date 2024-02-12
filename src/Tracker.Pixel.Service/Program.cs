using Microsoft.AspNetCore.Mvc;
using Tracker.Pixel.Service.Core;
using Tracker.Pixel.Service.Models;
using Tracker.Pixel.Service.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTrackingServiceConfigurations(builder.Configuration);
builder.WebHost.UseKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(3000);
});

var app = builder.Build();

app.MapGet("/track", 
    async ([FromServices]IMessageProducerService messageProducerService, HttpRequest request) =>
    {
        var ipAddress = request.HttpContext.Connection.RemoteIpAddress?.ToString();
        if (string.IsNullOrWhiteSpace(ipAddress)) return Results.BadRequest("Ip Address not present in Header");
        
        var trackingInfo = new TrackingInfo(
            DateTime.Now.ToUniversalTime(),
            ipAddress,
            request.Headers.Referer,
            request.Headers.UserAgent);

        await messageProducerService.ProduceMessageAsync(trackingInfo);

        return Results.Ok();
    });

app.Run();

public partial class Program { }