namespace SmartLockPlatform.Test.Integration.Members;

public class RegisterMemberInSiteByOtherMemberWithPermission : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly TestingWebAppFactory<Program> _factory;

    public RegisterMemberInSiteByOtherMemberWithPermission(TestingWebAppFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public Task Should_register_member_in_site_by_other_member_with_permission()
    {
        return Task.CompletedTask;
    }
}