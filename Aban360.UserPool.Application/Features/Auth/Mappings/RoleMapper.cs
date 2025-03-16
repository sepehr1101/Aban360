using Aban360.UserPool.Domain.Features.Auth.Dto.Commands;
using Aban360.UserPool.Domain.Features.Auth.Dto.Queries;
using Aban360.UserPool.Domain.Features.Auth.Entities;
using AutoMapper;

namespace Aban360.UserPool.Application.Features.Auth.Mappings
{
    public class RoleMapper : Profile
    {
        public RoleMapper()
        {
            CreateMap<RoleCreateDto, Role>();
            CreateMap<RoleDeleteDto, Role>();
            CreateMap<RoleUpdateDto, Role>();
            CreateMap<Role,RoleGetDto>();
            CreateMap<Role, UserRoleQueryDto>();
        }
    }
}
