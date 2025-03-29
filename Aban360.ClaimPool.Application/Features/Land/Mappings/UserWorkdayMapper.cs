using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Mappings
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