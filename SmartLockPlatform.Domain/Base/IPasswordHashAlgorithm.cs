namespace SmartLockPlatform.Domain.Base;

public interface IPasswordHashAlgorithm
{
    string HashPassword(string password);
    PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword);
}

public enum PasswordVerificationResult
{
    /// <summary>
    /// Indicates password verification failed.
    /// </summary>
    Failed = 0,

    /// <summary>
    /// Indicates password verification was successful.
    /// </summary>
    Success = 1,

    /// <summary>
    /// Indicates password verification was successful however the password was encoded using a deprecated algorithm
    /// and should be rehashed and updated.
    /// </summary>
    SuccessRehashNeeded = 2
}