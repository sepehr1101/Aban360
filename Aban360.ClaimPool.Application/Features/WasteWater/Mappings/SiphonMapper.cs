using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Mappings
{
    public class SiphonMapper : Profile
    {
        public SiphonMapper()
        {
            CreateMap<SiphonCreateDto, Siphon>();
            CreateMap<SiphonDeleteDto, Siphon>();
            CreateMap<SiphonUpdateDto, Siphon>();
            CreateMap<Siphon,SiphonGetDto>();
        }
    }
}
