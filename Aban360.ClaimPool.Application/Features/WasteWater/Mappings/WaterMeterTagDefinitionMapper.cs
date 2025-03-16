using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Domain.Features.Metering.Entities;
using AutoMapper;

namespace Aban360.ClaimPool.Application.Features.WasteWater.Mappings
{
   public class WaterMeterTagDefinitionMapper:Profile
    {
        public WaterMeterTagDefinitionMapper()
        {
            CreateMap<WaterMeterTagDefinitionCreateDto, WaterMeterTagDefinition>();
            CreateMap<WaterMeterTagDefinitionDeleteDto, WaterMeterTagDefinition>();
            CreateMap<WaterMeterTagDefinitionUpdateDto, WaterMeterTagDefinition>();
            CreateMap<WaterMeterTagDefinition,WaterMeterTagDefinitionGetDto>();
        }
    }
}
