using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Mappings
{
    public class SiphonTypeMapper : Profile
    {
        public SiphonTypeMapper()
        {
            CreateMap<SiphonTypeCreateDto, SiphonType>().ReverseMap();
            CreateMap<SiphonTypeDeleteDto, SiphonType>().ReverseMap();
            CreateMap<SiphonTypeUpdateDto, SiphonType>().ReverseMap();
            CreateMap<SiphonTypeGetDto, SiphonType>().ReverseMap();
        }
    }
}
