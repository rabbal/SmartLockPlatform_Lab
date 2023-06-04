using SmartLockPlatform.Application.Base;

namespace SmartLockPlatform.Application.Commands;

public record RegisterUserCommand(string Email, string FirstName, string LastName, string Password) : ICommand;