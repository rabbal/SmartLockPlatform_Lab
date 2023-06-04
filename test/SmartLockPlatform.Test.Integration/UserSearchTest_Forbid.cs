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

public class UserSearchTest_Forbid : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly TestingWebAppFactory<Program> _factory;

    public UserSearchTest_Forbid(TestingWebAppFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Should_not_search_users_with_term_when_unauthorized()
    {
        //Arrange
        var client = _factory
            .CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

        //Act
        var response = await client.GetAsync($"api/v1.0/users/search?search_term=user1");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}