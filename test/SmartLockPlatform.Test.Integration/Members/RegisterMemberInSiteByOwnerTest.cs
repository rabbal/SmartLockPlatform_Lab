using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Queries.DTO;

namespace SmartLockPlatform.Test.Integration.Members;

public class RegisterMemberInSiteByOwnerTest : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly TestingWebAppFactory<Program> _factory;
    private readonly HttpClient _client;

    public RegisterMemberInSiteByOwnerTest(TestingWebAppFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Should_register_member_in_site_by_owner()
    {
        //Arrange
        await RegisterUsers();
        await RegisterOwner();
        await PrepareBearerToken();
        var site = await RegisterSiteByOwner();
        var users = await SearchUsers();
        
        //Act, Assert
        foreach (var user in users)
        {
            var payload = new { UserId = user.Id, Alias = $"(site 1) {user.Email}", RoleIds = Array.Empty<long>() };
            var response = await _client.PostAsJsonAsync($"api/v1.0/sites/{site.Id}/members", payload);
          
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var member = await response.Content.ReadFromJsonAsync<MemberDTO>();
            Assert.NotNull(member);
            member.Alias.Should().Be($"(site 1) {user.Email}");
            member.User.Id.Should().Be(user.Id);
        }

        var members = await _client.GetFromJsonAsync<PaginatedList<MemberDTO>>($"api/v1.0/sites/{site.Id}/members?$include_count=true&$top=100");
        Assert.NotNull(members);
        members.Count.Should().Be(11);
    }

    private async Task<IReadOnlyList<UserDTO>> SearchUsers()
    {
        //Arrange
        const string term = "user1";

        //Act
        var response = await _client.GetAsync($"api/v1.0/users/search?search_term={term}");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var users = await response.Content.ReadFromJsonAsync<IReadOnlyList<UserDTO>>();
        Assert.NotNull(users);
        users.Count.Should().Be(11);
        users[^1].Email.Should().Be("user1@example.com");

        return users;
    }

    private async Task RegisterUsers()
    {
        for (var i = 1; i <= 20; i++)
        {
            //Arrange
            var payload = new
            {
                Email = $"user{i}@example.com", FirstName = i.ToString(), LastName = i.ToString(), Password = "@dmiN123"
            };

            //Act
            var response = await _client.PostAsJsonAsync("api/v1.0/auth/sign-up", payload);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }

    private async Task<SiteDTO> RegisterSiteByOwner()
    {
        //Arrange
        var payload = new { Name = "site 1" };

        //Act
        var response = await _client.PostAsJsonAsync("api/v1.0/sites", payload);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<SiteDTO>();
        Assert.NotNull(content);
        content.Id.Should().Be(1);
        content.Owner.Should().NotBeNull();
        content.Owner.Id.Should().Be(21);

        return content;
    }

    private async Task RegisterOwner()
    {
        //Arrange
        var payload = new
        {
            Email = "owner@example.com", FirstName = "owner", LastName = "site 1", Password = "@dmiN123"
        };

        //Act
        var response = await _client.PostAsJsonAsync("api/v1.0/auth/sign-up", payload);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task PrepareBearerToken()
    {
        //Arrange
        var payload = new { Email = "owner@example.com", Password = "@dmiN123" };

        //Act
        var response = await _client.PostAsJsonAsync("api/v1.0/auth/token", payload);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var token = await response.Content.ReadFromJsonAsync<Token>();
        token.Should().NotBeNull();
        token!.Value.Should().NotBeEmpty();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
    }
}