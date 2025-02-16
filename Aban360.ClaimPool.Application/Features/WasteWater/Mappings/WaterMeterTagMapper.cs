using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Mappings
{
    public class WaterMeterTagMapper : Profile
    {
        public WaterMeterTagMapper()
        {
            CreateMap<WaterMeterTagCreateDto, WaterMeterTag>().ReverseMap();
            CreateMap<WaterMeterTagDeleteDto, WaterMeterTag>().ReverseMap();
            CreateMap<WaterMeterTagUpdateDto, WaterMeterTag>().ReverseMap();
            CreateMap<WaterMeterTagGetDto, WaterMeterTag>().ReverseMap();
        }
    }
}
