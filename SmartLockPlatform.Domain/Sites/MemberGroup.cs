using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Domain.Sites;

public class MemberGroup : Entity
{
    private readonly List<Member> _members = new();

    public MemberGroup(string name)
    {
        Name = name;
    }

    public IReadOnlyList<Member> Members => _members.AsReadOnly();
    public string Name { get; set; }
}