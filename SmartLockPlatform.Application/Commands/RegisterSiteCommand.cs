using SmartLockPlatform.Application.Base;
using SmartLockPlatform.Application.Queries.DTO;
using SmartLockPlatform.Domain.Base;

namespace SmartLockPlatform.Application.Commands;

public class RegisterSiteCommand : ICommand<Result<SiteDTO>>
{
    public RegisterSiteCommand(string name)
    {
        Name = name;
    }

    public string Name { get; }
}