using Microsoft.Extensions.Options;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Commands;
using SmartLockPlatform.Domain.Base;
using SmartLockPlatform.Domain.Identity;

namespace SmartLockPlatform.Application.CommandHandlers;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IPasswordHashAlgorithm _hashAlgorithm;
    private readonly IOptions<IdentityOptions> _options;

    public RegisterUserCommandHandler(
        IAppDbContext dbContext,
        IPasswordHashAlgorithm hashAlgorithm,
        IOptions<IdentityOptions> options)
    {
        _dbContext = dbContext;
        _hashAlgorithm = hashAlgorithm;
        _options = options;
    }

    public async Task<Result> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var email = Email.Create(command.Email);
        if (email.Failed) return email;

        var firstName = new FirstName(command.FirstName);
        var lastName = new LastName(command.LastName);
        var user = new User(firstName, lastName, email.Value, false);

        var password = Password.Create(command.Password, _options.Value.Password);
        if (password.Failed) return password;

        user.SetPassword(password.Value, _hashAlgorithm);

        await _dbContext.Set<User>().AddAsync(user, cancellationToken);
        await _dbContext.Complete(cancellationToken);

        return Ok();
    }
}