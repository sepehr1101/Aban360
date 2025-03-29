using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Mappings
{
    public class UserLeaveMapper : Profile
    {

        public UserLeaveMapper()
        {
            CreateMap<UserLeaveCreateDto, UserLeave>();
            CreateMap<UserLeaveDeleteDto, UserLeave>();
            CreateMap<UserLeaveUpdateDto, UserLeave>();
            CreateMap<UserLeave, UserLeaveGetDto>();
        }
    }
}