namespace Tracker.Pixel.Service.Tests;

using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Xunit;

public class MinimalApiTests : IClassFixture<WebApplicationFactory<Program>>
{
private readonly WebApplicationFactory<Program> factory;

public MinimalApiTests(WebApplicationFactory<Program> factory)
{
    this.factory = factory;
}
    [Fact]
    public async Task OnGet_WhenIpAddressNotPresentInHttpContext_ShouldReturnBadRequest()
    {
        //Arrange
        var client = this.factory.CreateClient();

        var customHeaders = new Dictionary<string, string>
        {
            {"UserAgent", "Agent 2.3" },
            {"Referer", "google.com" }
        };

        var request = new HttpRequestMessage(HttpMethod.Get, "/track");

        foreach (var (key, value) in customHeaders)
        {
            request.Headers.Add(key, value);
        }
            
        //Act
        var response = await client.SendAsync(request);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact (Skip = "Still have to figure out if it is possible to spoof HttpContext")]
    public async Task OnGet_WhenIpAddressIsPresentInContext_ShouldReturnOk()
    {
        //Arrange
        var client = this.factory.CreateClient();

        var request = new HttpRequestMessage(HttpMethod.Get, "/track");

        var ip = IPAddress.Parse("199.9.9.100");
        
        var httpContext = new DefaultHttpContext
        {
            Connection =
            {
                RemoteIpAddress = IPAddress.Parse("199.9.9.100")
            },
            Request =
            {
                Method = "GET",
                Path = "/track",
                Headers =
                {
                    {"UserAgent", "Agent 2.3" },
                    {"Referer", "google.com" }
                },
            }
        };
        
        request.Properties["MS_HttpContext"] = httpContext;

        //Act
        var response = await client.SendAsync(request);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}