using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
using AutoMapper;

namespace Aban460.UserPool.UserLeavelication.Features.TimeTable.MUserLeaveings
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