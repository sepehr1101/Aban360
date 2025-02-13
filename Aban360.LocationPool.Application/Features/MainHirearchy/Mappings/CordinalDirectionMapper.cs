using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Queries;
using Aban360.LocationPool.Domain.Features.MainHirearchy.Entities;
using AutoMapper;

namespace Aban360.LocationPool.Application.Features.MainHirearchy.Mappings
{
    public class CordinalDirectionMapper:Profile
    {
        public CordinalDirectionMapper()
        {
            CreateMap<CordinalDirectionCreateDto, CordinalDirection>().ReverseMap();
            CreateMap<CordinalDirectionDeleteDto, CordinalDirection>().ReverseMap();
            CreateMap<CordinalDirectionUpdateDto, CordinalDirection>().ReverseMap();
            CreateMap<CordinalDirectionGetDto, CordinalDirection>().ReverseMap();
        }
    }
}
