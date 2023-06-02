using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Domain.Sites;

public class Lock : Entity
{
    private readonly List<LockRight> _rights = new();

    private Lock(string name, string uuid)
    {
        Uuid = uuid;
        Name = name;
    }

    public string Uuid { get; init; }
    public string Name { get; init; }
    public bool IsLocked { get; private set; }
    public bool IsOnline { get; private set; }
    public IReadOnlyList<LockRight> Rights => _rights.AsReadOnly();

    internal static Result<Lock> Create(string name, string uuid)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Fail<Lock>("The name is required for lock.");
        }

        if (string.IsNullOrWhiteSpace(uuid))
        {
            return Fail<Lock>("The uuid is required for lock.");
        }

        return new Lock(name, uuid);
    }
    internal void UnLock()
    {
        //TODO: check potential pre-conditions
        IsLocked = false;
    }

    internal void PutOnline()
    {
        IsOnline = true;
    }

    internal void TakeOffline()
    {
        IsOnline = false;
    }

    internal Result GrantRight(MemberGroup group, Timeframe? timeframe)
    {
        if (group is null) throw new ArgumentNullException(nameof(group));
        if (timeframe is null) throw new ArgumentNullException(nameof(timeframe));

        if (_rights.Exists(g => g.Group == group))
        {
            return Fail("The given group already exists.");
        }

        _rights.Add(new LockRight(group, timeframe));

        return Ok();
    }
    
    internal Result RevokeRight(MemberGroup group)
    {
        if (group is null) throw new ArgumentNullException(nameof(group));

        var right = _rights.Find(g => g.Group == group);
        if (right is null)
        {
            return Fail("The given group hasn't been found.");
        }

        _rights.Remove(right);

        return Ok();
    }

    public bool CanUnLock(Member member)
    {
        return _rights.Exists(right => right.Group.Members.Contains(member));
    }
}