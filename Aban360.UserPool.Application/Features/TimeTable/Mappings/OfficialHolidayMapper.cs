using Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands;
using Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries;
using Aban360.UserPool.Domain.Features.TimeTable.Entites;
using AutoMapper;

namespace Aban460.UserPool.OfficialHolidaylication.Features.TimeTable.MOfficialHolidayings
{
    public class OfficialHolidayMapper : Profile
    {

        public OfficialHolidayMapper()
        {
            CreateMap<OfficialHolidayCreateDto, OfficialHoliday>();
            CreateMap<OfficialHolidayDeleteDto, OfficialHoliday>();
            CreateMap<OfficialHolidayUpdateDto, OfficialHoliday>();
            CreateMap<OfficialHoliday, OfficialHolidayGetDto>();
        }
    }
}