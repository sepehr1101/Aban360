using Aban360.CalculationPool.Domain.Features.Rule.Dto.Commands;
using Aban360.CalculationPool.Domain.Features.Rule.Dto.Queries;
using Aban360.CalculationPool.Domain.Features.Rule.Entties;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Rule.Mappings
{
    public class TariffMapper : Profile
    {
        public TariffMapper()
        {
            CreateMap<TariffCreateDto, Tariff>().ReverseMap();
            CreateMap<TariffDeleteDto, Tariff>().ReverseMap();
            CreateMap<TariffUpdateDto, Tariff>().ReverseMap();

            CreateMap<Tariff, TariffGetDto>()
                .ForMember(dest => dest.OfferingTitle, x => x.MapFrom(m => m.Offering.Title))
                .ForMember(dest => dest.LineItemTypeTitle, x => x.MapFrom(m => m.LineItemType.Title));
        }
    }
}
