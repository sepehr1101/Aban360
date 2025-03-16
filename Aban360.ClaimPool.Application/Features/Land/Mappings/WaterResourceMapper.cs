using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Mappings
{
    public class WaterResourceMapper : Profile
    {
        public WaterResourceMapper()
        {
            CreateMap<WaterResourceCreateDto, WaterResource>();
            CreateMap<WaterResourceDeleteDto, WaterResource>();
            CreateMap<WaterResourceUpdateDto, WaterResource>();
            CreateMap<WaterResource,WaterResourceGetDto>();
        }
    }
   
}
