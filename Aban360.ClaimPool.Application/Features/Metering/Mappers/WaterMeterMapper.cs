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
            CreateMap<WaterMeterCreateDto, WaterMeter>().ReverseMap();
            CreateMap<WaterMeterDeleteDto, WaterMeter>().ReverseMap();
            CreateMap<WaterMeterUpdateDto, WaterMeter>().ReverseMap();
            CreateMap<WaterMeterGetDto, WaterMeter>().ReverseMap();
        }
    }
}
