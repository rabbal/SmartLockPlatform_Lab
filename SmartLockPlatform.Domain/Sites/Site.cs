using SmartLockPlatform.Domain.Base;
using SmartLockPlatform.Domain.Identity;
using SmartLockPlatform.Domain.Sites.Events;

namespace SmartLockPlatform.Domain.Sites;

public sealed class Site : AggregateRoot
{
    private readonly List<Lock> _locks = new();
    private readonly List<Member> _members = new();
    private readonly List<MemberGroup> _groups = new();
    private readonly List<Role> _roles = new();

#pragma warning disable CS8618
    private Site() //ORM
#pragma warning restore CS8618
    {
    }

    public Site(string name, User owner)
    {
        Name = name;
        Owner = owner;
        // Timezone = timezone;
    }

    public string Name { get; init; }

    //public TimeZoneInfo Timezone { get; init; }
    public User Owner { get; init; }
    public IReadOnlyList<Lock> Locks => _locks.AsReadOnly();
    public IReadOnlyList<Member> Members => _members.AsReadOnly();
    public IReadOnlyList<MemberGroup> Groups => _groups.AsReadOnly();
    public IReadOnlyList<Role> Roles => _roles.AsReadOnly();

    public Result AddLock(string name, string uuid)
    {
        if (_locks.Exists(_ => _.Name == name))
        {
            return Fail("The lock with given name already exists.");
        }

        if (_locks.Exists(_ => _.Uuid == uuid))
        {
            return Fail("The lock with given uuid already exists.");
        }

        var result = Lock.Create(name, uuid);
        if (result.Failed) return result;

        _locks.Add(result.Data);

        return Ok();
    }

    public Result UnLock(string uuid, User user)
    {
        var @lock = _locks.Find(_ => _.Uuid == uuid);

        if (@lock is null)
        {
            return Fail("There is no lock with given uuid.");
        }

        var member = _members.Find(u => u.User == user);
        if (member is null)
        {
        }

        @lock.UnLock();

        AddDomainEvent(new LockUnLockedDomainEvent(@lock));

        return Ok();
    }

    public Lock? FindLock(string uuid)
    {
        return _locks.Find(_ => _.Uuid == uuid);
    }

    public Result PutLockOnline(string uuid)
    {
        var @lock = FindLock(uuid);
        if (@lock is null) return LockNotFound(uuid);

        @lock.PutOnline();

        return Ok();
    }

    public Result TakeLockOffline(string uuid)
    {
        var @lock = FindLock(uuid);
        if (@lock is null) return LockNotFound(uuid);

        @lock.TakeOffline();

        return Ok();
    }

    public Result<Member> RegisterMember(User user, string? alias, IEnumerable<long> roleIds)
    {
        if (_members.Exists(u => u.User == user))
        {
            return Fail<Member>("The user already exists.");
        }

        var member = new Member(user, alias);
        _members.Add(member);

        _roles.FindAll(role => roleIds.Contains(role.Id))
            .ForEach(role => role.RegisterMember(member));

        return member;
    }

    public Result RemoveUser(User user)
    {
        var member = _members.Find(u => u.User == user);
        if (member is null)
        {
            return Fail("The given user does not exists.");
        }

        _members.Remove(member);

        return Ok();
    }

    public Result GrantRightToLock(string uuid, MemberGroup group, Timeframe timeframe)
    {
        var @lock = FindLock(uuid);
        return @lock is null ? LockNotFound(uuid) : @lock.GrantRight(group, timeframe);
    }

    public Result RevokeRightFromLock(string uuid, MemberGroup group)
    {
        var @lock = FindLock(uuid);
        return @lock is null ? LockNotFound(uuid) : @lock.RevokeRight(group);
    }

    private static Result LockNotFound(string uuid) => Fail($"There is no lock with given uuid [{uuid}].");
}