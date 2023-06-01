using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Domain.Sites.Events;

public class LockUnLockedDomainEvent : DomainEvent
{
    public LockUnLockedDomainEvent(Lock @lock)
    {
        Lock = @lock;
    }

    public Lock Lock { get; init; }
}