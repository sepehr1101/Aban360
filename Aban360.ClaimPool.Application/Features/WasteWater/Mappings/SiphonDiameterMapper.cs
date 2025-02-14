using Aban360.ClaimPool.Domain.Features.WasteWater;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Mappings
{
    public class SiphonDiameterMapper : Profile
    {
        public SiphonDiameterMapper()
        {
            CreateMap<SiphonDiameterCreateDto, SiphonDiameter>().ReverseMap();
            CreateMap<SiphonDiameterDeleteDto, SiphonDiameter>().ReverseMap();
            CreateMap<SiphonDiameterUpdateDto, SiphonDiameter>().ReverseMap();
            CreateMap<SiphonDiameterGetDto, SiphonDiameter>().ReverseMap();
        }
    }
}
