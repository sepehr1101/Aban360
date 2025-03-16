using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Domain.Features.Rule.Entities;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Mappings
{
    public class TariffConstantMapper : Profile
    {
        public TariffConstantMapper()
        {
            CreateMap<TariffConstantCreateDto, TariffConstant>();
            CreateMap<TariffConstantDeleteDto, TariffConstant>();
            CreateMap<TariffConstantUpdateDto, TariffConstant>();
            CreateMap<TariffConstant, TariffConstantGetDto>();
        }
    }
}
