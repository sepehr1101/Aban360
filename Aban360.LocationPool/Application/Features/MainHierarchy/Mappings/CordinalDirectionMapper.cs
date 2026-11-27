using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHierarchy.Entities;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHierarchy.Mappings
{
    public class CordinalDirectionMapper:Profile
    {
        public CordinalDirectionMapper()
        {
            CreateMap<CordinalDirectionCreateDto, CordinalDirection>();
            CreateMap<CordinalDirectionDeleteDto, CordinalDirection>();
            CreateMap<CordinalDirectionUpdateDto, CordinalDirection>();
            CreateMap<CordinalDirection,CordinalDirectionGetDto>();
        }
    }
}
