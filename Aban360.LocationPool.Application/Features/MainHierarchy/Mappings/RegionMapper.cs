using Aban360.LocationPool.Domain.Features.MainHierarchy;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Mappings
{
    public class RegionMapper:Profile
    {
        public RegionMapper()
        {
            CreateMap<RegionCreateDto, Region>().ReverseMap();
            CreateMap<RegionDeleteDto, Region>().ReverseMap();
            CreateMap<RegionUpdateDto, Region>().ReverseMap();
            CreateMap<RegionGetDto, Region>().ReverseMap();
        }
    }
}
