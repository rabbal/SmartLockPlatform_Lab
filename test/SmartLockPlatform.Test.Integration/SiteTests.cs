using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using SmartLockPlatform.Application.Queries.DTO;

namespace SmartLockPlatform.Test.Integration;

public class SiteTests : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly TestingWebAppFactory<Program> _factory;

    public SiteTests(TestingWebAppFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Should_register_site()
    {
        //Arrange
        var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication(defaultScheme: "TestScheme")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "TestScheme", options => { });
                });
            })
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(scheme: "TestScheme");

        var payload = new { Name = "site 1" };

        //Act
        var response = await client.PostAsJsonAsync("api/v1.0/sites", payload);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<SiteDTO>();
        Assert.NotNull(content);
        content.Id.Should().Be(1);
        content.Owner.Should().NotBeNull();
        content.Owner.Id.Should().Be(-1);
    }
}