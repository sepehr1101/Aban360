using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using AutoMapper;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Mappings
{
    public class HeadquarterMapper:Profile
    {
        public HeadquarterMapper()
        {
            CreateMap<HeadquarterCreateDto, Headquarters>();
            CreateMap<HeadquarterDeleteDto, Headquarters>();
            CreateMap<HeadquarterUpdateDto, Headquarters>();
            CreateMap<Headquarters,HeadquarterGetDto>()
                .ForMember(dest => dest.ProvinceTitle, opt => opt.MapFrom(src => src.Province.Title));

        }
    }
}
