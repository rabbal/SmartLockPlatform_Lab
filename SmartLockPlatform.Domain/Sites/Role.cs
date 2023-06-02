using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Domain.Sites;

public class Role : Entity
{
    private readonly List<RolePermission> _permissions = new();
    private readonly List<Member> _members = new();

#pragma warning disable CS8618
    private Role() //ORM
#pragma warning restore CS8618
    {
    }

    public Role(string name, IEnumerable<string> permissions)
    {
        Name = name;
        _permissions = permissions.Select(p => new RolePermission(p)).ToList();
    }

    public string Name { get; init; }
    public IReadOnlyList<RolePermission> Permissions => _permissions.AsReadOnly();

    public void RemovePermission(string name)
    {
        _permissions.RemoveAll(p => p.Name == name);
    }

    public void AddPermission(string name)
    {
        if (_permissions.Exists(p => p.Name == name)) return;

        _permissions.Add(new RolePermission(name));
    }

    public void RegisterMember(Member member)
    {
        if (_members.Contains(member)) return;
        
        member.AssignRole(this);
        _members.Add(member);
    }
}