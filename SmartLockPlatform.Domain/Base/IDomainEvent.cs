namespace SmartLockPlatform.Domain.Base;

public interface IDomainEvent
{
    Guid Id { get; }
    DateTime DateTime { get; }
}