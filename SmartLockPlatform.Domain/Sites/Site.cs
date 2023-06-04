using SmartLockPlatform.Domain.Base;
using SmartLockPlatform.Domain.Identity;

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

    public Result<Lock> AddLock(string name, string uuid)
    {
        if (_locks.Exists(_ => _.Name == name))
        {
            return Fail<Lock>("The lock with given name already exists.");
        }

        if (_locks.Exists(_ => _.Uuid == uuid))
        {
            return Fail<Lock>("The lock with given uuid already exists.");
        }

        var result = Lock.Create(name, uuid);
        if (result.Failed) return result;

        _locks.Add(result.Data);

        return result.Data;
    }


    public Result CanUnLock(long id, long userId)
    {
        if (Owner.Id == userId) return Ok();

        var member = _members.Find(u => u.User.Id == userId);
        if (member is null)
        {
            return Fail($"There is no user with given userId [{userId}]");
        }

        var @lock = _locks.Find(_ => _.Id == id);
        if (@lock is null)
        {
            return Fail($"There is no lock with given id [{id}].");
        }

        if (@lock.CanUnLock(member) == false)
        {
            return Forbid($"You're not allowed to open the lock with id [{id}]");
        }

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

    public Result GrantRightToLock(long id, long groupId, Timeframe timeframe)
    {
        var @lock = _locks.Find(_ => _.Id == id);
        if (@lock is null) return Fail($"There is no lock with given id [{id}].");

        var group = _groups.Find(g => g.Id == groupId);
        if (group is null) return Fail($"There is no group with given id [{groupId}].");

        return @lock.GrantRight(group, timeframe);
    }

    public Result RevokeRightFromLock(long id, long groupId)
    {
        var @lock = _locks.Find(_ => _.Id == id);
        if (@lock is null) return Fail($"There is no lock with given id [{id}].");

        var group = _groups.Find(g => g.Id == groupId);
        if (group is null) return Fail($"There is no group with given id [{groupId}].");

        return @lock.RevokeRight(group);
    }

    public Result<Role> RegisterRole(string name, IEnumerable<string> permissions)
    {
        if (_roles.Exists(r => r.Name == name)) return Fail<Role>("A role with given name already exists.");

        var role = new Role(name, permissions);
        _roles.Add(role);

        return role;
    }


    public Result AddMemberToRole(long roleId, long memberId)
    {
        var role = _roles.Find(r => r.Id == roleId);
        if (role is null) return Fail($"There is no role with given roleId [{roleId}]");

        var member = _members.Find(m => m.Id == memberId);
        if (member is null) return Fail($"There is no member with given memberId [{memberId}]");

        role.RegisterMember(member);

        return Ok();
    }

    public Result RemoveMemberFromRole(long roleId, long memberId)
    {
        throw new NotImplementedException();
    }

    public Result AddMemberToGroup(long groupId, long memberId)
    {
        var group = _groups.Find(r => r.Id == groupId);
        if (group is null) return Fail($"There is no group with given groupId [{groupId}]");

        var member = _members.Find(m => m.Id == memberId);
        if (member is null) return Fail($"There is no member with given memberId [{memberId}]");

        group.AddMember(member);

        return Ok();
    }

    public Result RemoveMemberFromGroup(long groupId, long memberId)
    {
        throw new NotImplementedException();
    }

    public Result<MemberGroup> AddMemberGroup(string name)
    {
        if (_groups.Exists(g => g.Name == name)) return Fail<MemberGroup>("A group with given name already exists.");

        var group = new MemberGroup(name);
        _groups.Add(group);

        return group;
    }
}