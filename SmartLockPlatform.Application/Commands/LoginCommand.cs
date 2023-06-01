using System.Security.Claims;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Application.Commands;

public class LoginCommand : ICommand<Result<IReadOnlyList<Claim>>>
{
    public LoginCommand(string email, string password, bool logoutEverywhere = true)
    {
        Email = email;
        Password = password;
        LogoutEverywhere = logoutEverywhere;
    }

    public string Email { get; }
    public string Password { get; }
    public bool LogoutEverywhere { get; }
}