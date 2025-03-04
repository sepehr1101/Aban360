using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Mappings
{
    public class TariffCalculationModeMapper:Profile
    {
        public TariffCalculationModeMapper()
        {
            CreateMap<TariffCalculationModeCreateDto, TariffCalculationMode>().ReverseMap();
            CreateMap<TariffCalculationModeDeleteDto, TariffCalculationMode>().ReverseMap();
            CreateMap<TariffCalculationModeUpdateDto, TariffCalculationMode>().ReverseMap();
            CreateMap<TariffCalculationModeGetDto, TariffCalculationMode>().ReverseMap();
        }
    }
}
