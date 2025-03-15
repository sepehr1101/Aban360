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
            CreateMap<WaterResourceCreateDto, WaterResource>().ReverseMap();
            CreateMap<WaterResourceDeleteDto, WaterResource>().ReverseMap();
            CreateMap<WaterResourceUpdateDto, WaterResource>().ReverseMap();
            CreateMap<WaterResourceGetDto, WaterResource>().ReverseMap();
        }
    }
   
}
