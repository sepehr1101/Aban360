using Aban360.LocationPool.Domain.Features.MainHierarchy;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Mappings
{
    public class HeadquarterMapper:Profile
    {
        public HeadquarterMapper()
        {
            CreateMap<HeadquarterCreateDto, Headquarters>().ReverseMap();
            CreateMap<HeadquarterDeleteDto, Headquarters>().ReverseMap();
            CreateMap<HeadquarterUpdateDto, Headquarters>().ReverseMap();
            CreateMap<HeadquarterGetDto, Headquarters>().ReverseMap();
        }
    }
}
