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
            CreateMap<HeadquarterCreateDto, Headquarters>()
                .ReverseMap();

            CreateMap<HeadquarterDeleteDto, Headquarters>()
                .ReverseMap();

            CreateMap<HeadquarterUpdateDto, Headquarters>()
                .ReverseMap();
            
            CreateMap<HeadquarterGetDto, Headquarters>()
                .ReverseMap()
                .ForMember(dest => dest.ProvinceTitle, opt => opt.MapFrom(src => src.Province.Title));

        }
    }
}
