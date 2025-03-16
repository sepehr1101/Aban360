using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Mappings
{
    public class ZoneMapper : Profile
    {
        public ZoneMapper()
        {
            CreateMap<ZoneCreateDto, Zone>();
            CreateMap<ZoneDeleteDto, Zone>();
            CreateMap<ZoneUpdateDto, Zone>();
            CreateMap<Zone,ZoneGetDto>()
                .ForMember(dest => dest.RegionTitle, opt => opt.MapFrom(src => src.Region.Title));

        }
    }
}
