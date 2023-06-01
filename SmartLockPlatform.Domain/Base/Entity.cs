
namespace SmartLockPlatform.Domain.Base;

public interface IEntity
{
}

public interface IAggregateRoot : IEntity
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void EmptyDomainEvents();
}

public abstract class AggregateRoot : AggregateRoot<long>
{
    protected AggregateRoot()
    {
    }

    protected AggregateRoot(long id)
        : base(id)
    {
    }
}

public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot where TKey : IEquatable<TKey>, IComparable<TKey>
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public uint Version { get; protected set; } = default!;

    protected AggregateRoot()
    {
    }

    protected AggregateRoot(TKey id)
        : base(id)
    {
    }

    public virtual void EmptyDomainEvents() => _domainEvents.Clear();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    protected void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);
}

public abstract class Entity : Entity<long>
{
    protected Entity()
    {
    }

    protected Entity(long id)
        : base(id)
    {
    }
}

public abstract class Entity<TKey> : IEntity
    where TKey : IEquatable<TKey>
{
    private int? _hash;
    public TKey Id { get; protected set; } = default!;

    protected Entity()
    {
    }

    protected Entity(TKey id) : this()
    {
        Id = id;
    }

    public override int GetHashCode()
    {
        if (IsTransient()) return base.GetHashCode();

        _hash ??= (GetUnproxiedType().ToString() + Id).GetHashCode() ^ 31;

        return _hash.Value;
    }

    private bool IsTransient()
    {
        return EqualityComparer<TKey>.Default.Equals(Id, default);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TKey> instance) return false;

        if (ReferenceEquals(this, instance)) return true;

        if (GetUnproxiedType() != instance.GetUnproxiedType()) return false;

        if (IsTransient() || instance.IsTransient()) return false;

        return Id.Equals(instance.Id);
    }

    public override string ToString() => $"[{GetUnproxiedType().Name}: {Id}]";

    public static bool operator ==(Entity<TKey>? left, Entity<TKey>? right) => Equals(left, right);

    public static bool operator !=(Entity<TKey>? left, Entity<TKey>? right) => !(left == right);

    protected virtual Type GetUnproxiedType()
    {
        return ValueObject.GetUnproxiedType(this);
    }
}