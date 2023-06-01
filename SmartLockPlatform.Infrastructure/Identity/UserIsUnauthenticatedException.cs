
// ReSharper disable InconsistentNaming

namespace SmartLockPlatform.Infrastructure.Identity;

public class UserIsUnauthenticatedException : Exception
{
    public UserIsUnauthenticatedException(string message) : base(message)
    {
    }
}