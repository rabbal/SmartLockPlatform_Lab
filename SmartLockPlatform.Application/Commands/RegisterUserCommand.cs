using SmartLockPlatform.Application.Base;

namespace SmartLockPlatform.Application.Commands;

public class RegisterUserCommand : ICommand
{
    public RegisterUserCommand(string email, string firstName, string lastName, string password)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Password = password;
    }

    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Password { get; }
}