using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.Metering.Mappers
{
    public class WaterMeterMapper : Profile
    {
        public WaterMeterMapper()
        {
            CreateMap<WaterMeterCreateDto, WaterMeter>();
            CreateMap<WaterMeterDeleteDto, WaterMeter>();
            CreateMap<WaterMeterUpdateDto, WaterMeter>();
            CreateMap<WaterMeter,WaterMeterGetDto>();
        }
    }
}
