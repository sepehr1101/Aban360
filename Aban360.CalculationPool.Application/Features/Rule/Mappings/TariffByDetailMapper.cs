using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Mappings
{
    public class TariffByDetailMapper:Profile
    {
        public TariffByDetailMapper()
        {
            CreateMap<TariffByDetailCreateDto, TariffByDetail>();
            CreateMap<TariffByDetailDeleteDto, TariffByDetail>();
            CreateMap<TariffByDetailUpdateDto, TariffByDetail>();
            CreateMap<TariffByDetailCreateDto, TariffByDetail>();
        }
    }
}
