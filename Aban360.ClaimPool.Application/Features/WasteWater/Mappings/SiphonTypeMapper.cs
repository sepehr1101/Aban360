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
            CreateMap<SiphonTypeCreateDto, SiphonType>();
            CreateMap<SiphonTypeDeleteDto, SiphonType>();
            CreateMap<SiphonTypeUpdateDto, SiphonType>();
            CreateMap<SiphonType,SiphonTypeGetDto>();
        }
    }
}
