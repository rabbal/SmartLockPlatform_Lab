using System.Net.Http.Json;

namespace SmartLockPlatform.Test.Integration;

public class AuthTests : IClassFixture<IntegrationTestFactory<Program>>
{
    private readonly IntegrationTestFactory<Program> _factory;

    public AuthTests(IntegrationTestFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Should_register_user_with_credentials()
    {
        //Arrange
        var client = _factory.CreateClient();
        var payload = new { Email = "test@example.com", FirstName = "test", LastName = "user1", Password = "@dmiN123" };
       
        //Act
        var response = await client.PostAsJsonAsync("api/v1.0/auth/sign-up", payload);

        //Assert
        response.EnsureSuccessStatusCode();
    }
}