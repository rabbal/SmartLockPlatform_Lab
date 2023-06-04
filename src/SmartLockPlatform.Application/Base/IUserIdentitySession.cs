using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

// ReSharper disable InconsistentNaming

namespace SmartLockPlatform.Application.Base;

public interface IUserIdentitySession
{
    bool IsAdmin { get; }

    [MemberNotNullWhen(true, nameof(UserId), nameof(UserDisplayName))]
    bool IsAuthenticated { get; }

    long UserId { get; }
    string? UserDisplayName { get; }
    string? UserBrowserName { get; }
    string? UserIP { get; }
    IReadOnlyList<Claim> Claims { get; }

    [MemberNotNull(nameof(UserId))]
    void ThrowIfUnauthenticated();

    IDisposable Use(IEnumerable<Claim> claims, string ip);
}