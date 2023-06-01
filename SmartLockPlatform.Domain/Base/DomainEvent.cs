namespace SmartLockPlatform.Domain.Base;

public abstract class DomainEvent : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime DateTime { get; } = DateTime.UtcNow;
}