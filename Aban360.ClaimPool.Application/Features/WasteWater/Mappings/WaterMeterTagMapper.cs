using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Mappings
{
    public class WaterMeterTagMapper : Profile
    {
        public WaterMeterTagMapper()
        {
            CreateMap<WaterMeterTagCreateDto, WaterMeterTag>();
            CreateMap<WaterMeterTagDeleteDto, WaterMeterTag>();
            CreateMap<WaterMeterTagUpdateDto, WaterMeterTag>();
            CreateMap<WaterMeterTag,WaterMeterTagGetDto>();
        }
    }
}
