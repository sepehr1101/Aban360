using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
using AutoMapper;

namespace Aban460.UserPool.UserWorkdaylication.Features.TimeTable.MUserWorkdayings
{
    public class UserWorkdayMapper : Profile
    {

        public UserWorkdayMapper()
        {
            CreateMap<UserWorkdayCreateDto, UserWorkday>();
            CreateMap<UserWorkdayDeleteDto, UserWorkday>();
            CreateMap<UserWorkdayUpdateDto, UserWorkday>();
            CreateMap<UserWorkday, UserWorkdayGetDto>();
        }
    }
}