using Aban360.LocationPool.Domain.Features.MainHierarchy;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Mappings
{
    public class MunicipalityMapper:Profile
    {
        public MunicipalityMapper()
        {
            CreateMap<MunicipalityCreateDto, Municipality>().ReverseMap();
            CreateMap<MunicipalityDeleteDto, Municipality>().ReverseMap();
            CreateMap<MunicipalityUpdateDto, Municipality>().ReverseMap();
            CreateMap<MunicipalityGetDto, Municipality>().ReverseMap();
        }
    }
}
