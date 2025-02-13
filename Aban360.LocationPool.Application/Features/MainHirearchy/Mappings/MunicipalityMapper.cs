using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Mappings
{
    public class MunicipalityMapper : Profile
    {
        public MunicipalityMapper()
        {
            CreateMap<MunicipalityCreateDto, Municipality>()
                .ReverseMap();

            CreateMap<MunicipalityDeleteDto, Municipality>()
                .ReverseMap();

            CreateMap<MunicipalityUpdateDto, Municipality>()
                .ReverseMap();

            CreateMap<MunicipalityGetDto, Municipality>()
                .ReverseMap()
                .ForMember(dest => dest.ZoneTitle, opt => opt.MapFrom(src => src.Zone.Title));

        }
    }
}
