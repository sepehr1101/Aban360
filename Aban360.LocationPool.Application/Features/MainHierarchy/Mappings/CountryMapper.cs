using Aban360.LocationPool.Domain.Features.MainHierarchy;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Mappings
{
    public class CountryMapper : Profile
    {
        public CountryMapper()
        {
            CreateMap<CountryCreateDto, Country>().ReverseMap();
            CreateMap<CountryDeleteDto, Country>().ReverseMap();
            CreateMap<CountryUpdateDto, Country>().ReverseMap();    
            CreateMap<CountryGetDto, Country>().ReverseMap();    
        }
    }
}
