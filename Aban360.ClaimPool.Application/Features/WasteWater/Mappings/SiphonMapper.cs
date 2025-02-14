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
            CreateMap<SiphonCreateDto, Siphon>().ReverseMap();
            CreateMap<SiphonDeleteDto, Siphon>().ReverseMap();
            CreateMap<SiphonUpdateDto, Siphon>().ReverseMap();
            CreateMap<SiphonGetDto, Siphon>().ReverseMap();
        }
    }
}
