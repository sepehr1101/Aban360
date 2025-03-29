using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Mappings
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