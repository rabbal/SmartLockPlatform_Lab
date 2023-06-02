using System.Security.Claims;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Application.Commands;

public record LoginCommand(string Email, string Password, bool LogoutEverywhere = true) : ICommand<Result<IReadOnlyList<Claim>>>;