using Aban360.LocationPool.Domain.Features.MainHierarchy;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Mappings
{
    public class ProvinceMapper : Profile
    {
        public ProvinceMapper()
        {
            CreateMap<ProvinceCreateDto, Province>()
                .ReverseMap();

            CreateMap<ProvinceDeleteDto, Province>()
                .ReverseMap();

            CreateMap<ProvinceUpdateDto, Province>()
                .ReverseMap();

            CreateMap<ProvinceGetDto, Province>()
                .ReverseMap()
                .ForMember(dest => dest.CordinalDirectionTitle, opt => opt.MapFrom(src => src.CordinalDirection.Title))
                .ForMember(dest => dest.CountryTitle, opt => opt.MapFrom(src => src.Country.Title));

        }
    }
}
