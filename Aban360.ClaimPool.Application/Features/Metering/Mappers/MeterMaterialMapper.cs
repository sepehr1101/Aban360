using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Mappers
{
    public class MeterMaterialMapper : Profile
    {
        public MeterMaterialMapper()
        {
            CreateMap<MeterMaterialCreateDto, MeterMaterial>().ReverseMap();
            CreateMap<MeterMaterialDeleteDto, MeterMaterial>().ReverseMap();
            CreateMap<MeterMaterialUpdateDto, MeterMaterial>().ReverseMap();
            CreateMap<MeterMaterialGetDto, MeterMaterial>().ReverseMap();
        }
    }
}
