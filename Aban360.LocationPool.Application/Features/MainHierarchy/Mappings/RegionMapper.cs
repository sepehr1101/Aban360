using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using AutoMapper;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Mappings
{
    public class RegionMapper:Profile
    {
        public RegionMapper()
        {
            CreateMap<RegionCreateDto, Region>();
            CreateMap<RegionDeleteDto, Region>();
            CreateMap<RegionUpdateDto, Region>();
            CreateMap<Region,RegionGetDto>()
                .ForMember(dest => dest.HeadquartersTitle, opt => opt.MapFrom(src => src.Headquarters.Title));

        }
    }
}
