using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Mappings
{
    public class ZoneMapper : Profile
    {
        public ZoneMapper()
        {
            CreateMap<ZoneCreateDto, Zone>()
                .ReverseMap();

            CreateMap<ZoneDeleteDto, Zone>()
                .ReverseMap();

            CreateMap<ZoneUpdateDto, Zone>()
                .ReverseMap();

            CreateMap<ZoneGetDto, Zone>()
                .ReverseMap()
                .ForMember(dest => dest.RegionTitle, opt => opt.MapFrom(src => src.Region.Title));

        }
    }
}
