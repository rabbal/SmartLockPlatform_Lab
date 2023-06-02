using AutoMapper;
using SmartLockPlatform.Application.Commands;
using SmartLockPlatform.Application.Queries;
using SmartLockPlatform.Host.Controllers.V1.Models;

namespace SmartLockPlatform.Host.Controllers.V1.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterUserDTO, RegisterUserCommand>();
        CreateMap<LoginDTO, LoginCommand>();
        CreateMap<RegisterSiteDTO, RegisterSiteCommand>();
        CreateMap<PaginatedListQueryParams, ListSitesRequest>();
        CreateMap<PaginatedListQueryParams, ListMembersRequest>();
        CreateMap<RegisterMemberDTO, RegisterMemberCommand>();
    }
}