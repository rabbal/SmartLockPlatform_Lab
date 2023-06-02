using System.Globalization;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Commands;
using SmartLockPlatform.Application.Identity;
using SmartLockPlatform.Domain.Base;
using SmartLockPlatform.Domain.Identity;

namespace SmartLockPlatform.Application.CommandHandlers;

internal class LoginCommandHandler : ICommandHandler<LoginCommand, Result<IReadOnlyList<Claim>>>
{
    private readonly IAppDbContext _dbContext;
    private readonly IDateTime _dateTime;
    private readonly IPasswordHashAlgorithm _hashAlgorithm;
    private readonly IdentityOptions _options;

    public LoginCommandHandler(
        IAppDbContext dbContext,
        IDateTime dateTime,
        IPasswordHashAlgorithm hashAlgorithm,
        IOptions<IdentityOptions> options)
    {
        _dbContext = dbContext;
        _dateTime = dateTime;
        _hashAlgorithm = hashAlgorithm;
        _options = options.Value;
    }

    public async Task<Result<IReadOnlyList<Claim>>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Set<User>()
            .FindWithCriteria(u => u.Email.Value == command.Email, cancellationToken);

        if (user?.PasswordHash is null)
        {
            return Fail<IReadOnlyList<Claim>>("The provided credentials is invalid.");
        }

        if (user.IsLocked(_dateTime.Now))
        {
            return Fail<IReadOnlyList<Claim>>("This account has been locked out due to failed login attempts");
        }

        var passwordVerification = _hashAlgorithm.VerifyHashedPassword(user.PasswordHash, command.Password);
        if (passwordVerification == PasswordVerificationResult.Failed)
        {
            user.Lock(_options.Lockout, _dateTime.Now);
            return Fail<IReadOnlyList<Claim>>("The provided credentials is invalid");
        }

        if (passwordVerification == PasswordVerificationResult.SuccessRehashNeeded)
        {
            var password = Password.Create(command.Password, _options.Password);
            if (password.Failed == false) user.SetPassword(password.Data, _hashAlgorithm);
        }

        if (!user.IsActive)
        {
            return Fail<IReadOnlyList<Claim>>("This account has been deactivated");
        }

        user.ResetLockout();

        if (command.LogoutEverywhere)
        {
            user.LogoutEverywhere();
        }

        var claims = new List<Claim>
        {
            new(UserClaimTypes.UserId, user.Id.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64),
            new(UserClaimTypes.Email, user.Email.ToString(), ClaimValueTypes.String),
            new(UserClaimTypes.Name, user.Email.ToString(), ClaimValueTypes.String),
            new(UserClaimTypes.DisplayName, $"{user.FirstName} {user.LastName}", ClaimValueTypes.String),
            new(UserClaimTypes.SecurityStampToken, user.SecurityStampToken, ClaimValueTypes.String),
            new(UserClaimTypes.IsAdmin, user.IsAdmin.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Boolean)
        };

        return claims;
    }
}