namespace SmartLockPlatform.Host.Authentication;

public class TokenOptions
{
    public string SigningKey { set; get; } = default!;
    public string Issuer { set; get; } = default!;
    public string Audience { set; get; } = default!;
    public TimeSpan TokenExpiration { set; get; }
    public bool LoginFromSameUserEnabled { set; get; }
    public bool LogoutEverywhereEnabled { set; get; }
}