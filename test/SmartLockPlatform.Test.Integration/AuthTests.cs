using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using FluentAssertions;

namespace SmartLockPlatform.Test.Integration;

public class AuthTests : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly TestingWebAppFactory<Program> _factory;

    public AuthTests(TestingWebAppFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Should_register_user_with_credentials()
    {
        //Arrange
        var client = _factory.CreateClient();
        var payload = new
            { Email = "test1@example.com", FirstName = "test", LastName = "user1", Password = "@dmiN123" };

        //Act
        var response = await client.PostAsJsonAsync("api/v1.0/auth/sign-up", payload);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_get_access_token_with_super_admin_credentials()
    {
        //Arrange
        var client = _factory.CreateClient();
        var payload = new { Email = "admin@example.com", Password = "@dmiN123" };

        //Act
        var response = await client.PostAsJsonAsync("api/v1.0/auth/token", payload);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var token = await response.Content.ReadFromJsonAsync<Token>();
        token.Should().NotBeNull();
        token!.Value.Should().NotBeEmpty();
    }
}

public class Token
{
    [JsonPropertyName("access_token")] public string Value { get; init; } = default!;
}