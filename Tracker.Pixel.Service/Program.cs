using Microsoft.AspNetCore.Mvc;
using Tracker.Pixel.Service.Core;
using Tracker.Pixel.Service.Models;
using Tracker.Pixel.Service.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTrackingServiceConfigurations(builder.Configuration);

var app = builder.Build();

app.MapGet("/track", 
    async ([FromServices]IMessageProducerService messageProducerService, HttpRequest request) =>
    {
        var ipAddress = request.HttpContext.Connection.RemoteIpAddress?.ToString();
        if (!string.IsNullOrWhiteSpace(ipAddress))
        {
            var trackingInfo = new TrackingInfo(
                DateTime.Now.ToUniversalTime(),
                ipAddress,
                request.Headers.Referer,
                request.Headers.UserAgent);

            await messageProducerService.ProduceMessageAsync(trackingInfo);
        }
        
        return "";
    });

app.Run("http://localhost:3000");