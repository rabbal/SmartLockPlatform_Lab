using AutoMapper;
using SmartLockPlatform.Application.Commands;
using SmartLockPlatform.Host.Controllers.V1.Models;

namespace SmartLockPlatform.Host.Controllers.V1.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterUserDTO, RegisterUserCommand>();
        CreateMap<LoginDTO, LoginCommand>();
        CreateMap<RegisterSiteDTO, RegisterSiteCommand>();
        CreateMap<RegisterMemberDTO, RegisterMemberCommand>();
        CreateMap<RegisterLockDTO, RegisterLockCommand>();
        CreateMap<UnLockDTO, UnLockCommand>();
        CreateMap<RegisterRoleDTO, RegisterRoleCommand>();
        CreateMap<ManipulateMembersOfRoleDTO, ManipulateMembersOfRoleCommand>();
        CreateMap<ManipulateMembersOfGroupDTO, ManipulateMembersOfGroupCommand>();
        CreateMap<RegisterMemberGroupDTO, RegisterMemberGroupCommand>();
        CreateMap<GrantRightToLockDTO, GrantRightToLockCommand>();
    }
}