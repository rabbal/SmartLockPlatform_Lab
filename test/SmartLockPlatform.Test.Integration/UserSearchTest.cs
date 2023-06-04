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

public class UserSearchTest : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly TestingWebAppFactory<Program> _factory;

    public UserSearchTest(TestingWebAppFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Should_search_users_with_term()
    {
        //Arrange
        await RegisterUsers();

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

        //Act
        var response = await client.GetAsync($"api/v1.0/users/search?search_term=user1");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var users = await response.Content.ReadFromJsonAsync<IReadOnlyList<UserDTO>>();
        Assert.NotNull(users);
        users.Count.Should().Be(11);
        users[^1].Email.Should().Be("user1@example.com");
    }

    private async Task RegisterUsers()
    {
        for (var i = 1; i <= 20; i++)
        {
            //Arrange
            var client = _factory.CreateClient();
            var payload = new
            {
                Email = $"user{i}@example.com", FirstName = i.ToString(), LastName = i.ToString(), Password = "@dmiN123"
            };

            //Act
            var response = await client.PostAsJsonAsync("api/v1.0/auth/sign-up", payload);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}