using Microsoft.EntityFrameworkCore;
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

        if (await IsDuplicateUserWithEmail(command.Email, cancellationToken))
        {
            return Fail("The provided email address has been already taken by someone.");
        }
        
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

    private Task<bool> IsDuplicateUserWithEmail(string email, CancellationToken cancellationToken)
    {
        return _dbContext.Set<User>().AnyAsync(i => i.Email.Value == email, cancellationToken);
    }
}