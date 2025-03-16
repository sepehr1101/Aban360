using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Mappers
{
    public class MeterMaterialMapper : Profile
    {
        public MeterMaterialMapper()
        {
            CreateMap<MeterMaterialCreateDto, MeterMaterial>();
            CreateMap<MeterMaterialDeleteDto, MeterMaterial>();
            CreateMap<MeterMaterialUpdateDto, MeterMaterial>();
            CreateMap<MeterMaterial,MeterMaterialGetDto>();
        }
    }
}
