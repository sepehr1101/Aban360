using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using AutoMapper;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Mappings
{
    public class RegionMapper:Profile
    {
        public RegionMapper()
        {
            CreateMap<RegionCreateDto, Region>()
                .ReverseMap();

            CreateMap<RegionDeleteDto, Region>()
                .ReverseMap();

            CreateMap<RegionUpdateDto, Region>()
                .ReverseMap();

            CreateMap<RegionGetDto, Region>()
                .ReverseMap()
                .ForMember(dest => dest.HeadquartersTitle, opt => opt.MapFrom(src => src.Headquarters.Title));

        }
    }
}
