using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Domain.Sites;

public class RolePermission : Entity
{
    public RolePermission(string name)
    {
        Name = name;
    }

    public string Name { get; init; }
}