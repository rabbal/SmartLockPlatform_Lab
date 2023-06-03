using Refit;
using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Application.External;

public interface ISmartMqttClient
{
    [Post("/api/v1.0/unlock")]
    Task UnLock([Body] UnLockDTO model, CancellationToken cancellationToken = default);
}

public class UnLockDTO
{
    public long SiteId { get; init; }
    public long LockId { get; init; }
    public string LockUuid { get; init; } = default!;
    public long UserId { get; init; }
    //prevent duplicate lock opening by SmartLockPlatform.Mqtt
    public string IdempotentId { get; init; } = default!;
}

public class SmartMqttClientOptions
{
    public string BasePath { get; set; } = default!;
}