using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using AutoMapper;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Mappings
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
