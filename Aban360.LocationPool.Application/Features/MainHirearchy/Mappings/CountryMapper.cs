using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using AutoMapper;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Mappings
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
