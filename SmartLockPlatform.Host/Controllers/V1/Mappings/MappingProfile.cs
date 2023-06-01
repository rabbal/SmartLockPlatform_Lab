using AutoMapper;
using SmartLockPlatform.Application.Commands;

namespace SmartLockPlatform.Host.Controllers.V1.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterUserDTO, RegisterUserCommand>();
    }
}