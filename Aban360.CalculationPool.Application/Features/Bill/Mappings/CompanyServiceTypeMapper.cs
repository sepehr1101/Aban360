using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Mappings
{
    public class CompanyServiceTypeMapper : Profile
    {
        public CompanyServiceTypeMapper()
        {
            CreateMap<CompanyServiceTypeCreateDto, CompanyServiceType>();
            CreateMap<CompanyServiceTypeDeleteDto, CompanyServiceType>();
            CreateMap<CompanyServiceTypeUpdateDto, CompanyServiceType>();
            CreateMap< CompanyServiceType, CompanyServiceTypeGetDto>()
                .ForMember(dest => dest.TariffCalculationModeTitle, m => m.MapFrom(o => o.TariffCalculationMode.Title));
        }
    }
}
