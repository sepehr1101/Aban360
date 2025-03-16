using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Land.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Land.Mappings
{
    public class EstateWaterResourceMapper : Profile
    {
        public EstateWaterResourceMapper()
        {
            CreateMap<EstateWaterResourceCreateDto, EstateWaterResource>();
            CreateMap<EstateWaterResourceDeleteDto, EstateWaterResource>();
            CreateMap<EstateWaterResourceUpdateDto, EstateWaterResource>();

            CreateMap<EstateWaterResource, EstateWaterResourceGetDto>()
                .ForMember(dest => dest.WaterResourceTitle, x => x.MapFrom(mem => mem.WaterResource.Title))
                .ForMember(dest => dest.EstatePostalCode, x => x.MapFrom(mem => mem.Estate.PostalCode));
        }
    }

}
