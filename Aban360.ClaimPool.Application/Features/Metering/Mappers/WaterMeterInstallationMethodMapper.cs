using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Mappers
{
    public class WaterMeterInstallationMethodMapper : Profile
    {
        public WaterMeterInstallationMethodMapper()
        {
            CreateMap<WaterMeterInstallationMethodCreateDto, WaterMeterInstallationMethod>();
            CreateMap<WaterMeterInstallationMethodDeleteDto, WaterMeterInstallationMethod>();
            CreateMap<WaterMeterInstallationMethodUpdateDto, WaterMeterInstallationMethod>();
            CreateMap<WaterMeterInstallationMethod,WaterMeterInstallationMethodGetDto>();
        }
    }
}
