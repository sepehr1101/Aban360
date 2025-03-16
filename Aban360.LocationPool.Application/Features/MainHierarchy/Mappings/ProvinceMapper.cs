using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Mappings
{
    public class ProvinceMapper : Profile
    {
        public ProvinceMapper()
        {
            CreateMap<ProvinceCreateDto, Province>();
            CreateMap<ProvinceDeleteDto, Province>();
            CreateMap<ProvinceUpdateDto, Province>();
            CreateMap<Province,ProvinceGetDto>()
                .ForMember(dest => dest.CordinalDirectionTitle, opt => opt.MapFrom(src => src.CordinalDirection.Title))
                .ForMember(dest => dest.CountryTitle, opt => opt.MapFrom(src => src.Country.Title));

        }
    }
}
