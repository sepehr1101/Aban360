using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.WasteWater.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.WasteWater.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Mappings
{
    public class SiphonDiameterMapper : Profile
    {
        public SiphonDiameterMapper()
        {
            CreateMap<SiphonDiameterCreateDto, SiphonDiameter>();
            CreateMap<SiphonDiameterDeleteDto, SiphonDiameter>();
            CreateMap<SiphonDiameterUpdateDto, SiphonDiameter>();
            CreateMap<SiphonDiameter,SiphonDiameterGetDto>();
        }
    }
}
