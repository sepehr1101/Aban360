using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Mappers
{
    public class WaterMeterInstallationStructureMapper : Profile
    {
        public WaterMeterInstallationStructureMapper()
        {
            CreateMap<WaterMeterInstallationStructureCreateDto, WaterMeterInstallationStructure>();
            CreateMap<WaterMeterInstallationStructureDeleteDto, WaterMeterInstallationStructure>();
            CreateMap<WaterMeterInstallationStructureUpdateDto, WaterMeterInstallationStructure>();
            CreateMap<WaterMeterInstallationStructure,WaterMeterInstallationStructureGetDto>();
        }
    }
}
