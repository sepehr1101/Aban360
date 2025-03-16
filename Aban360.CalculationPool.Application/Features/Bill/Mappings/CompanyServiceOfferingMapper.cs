using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Mappings
{
    public class CompanyServiceOfferingMapper : Profile
    {
        public CompanyServiceOfferingMapper()
        {
            CreateMap<CompanyServiceOfferingCreateDto, CompanyServiceOffering>();
            CreateMap<CompanyServiceOfferingDeleteDto, CompanyServiceOffering>();
            CreateMap<CompanyServiceOfferingUpdateDto, CompanyServiceOffering>();
            CreateMap<CompanyServiceOffering,CompanyServiceOfferingGetDto>()
                    .ForMember(dest => dest.CompanyServiceTitle, m => m.MapFrom(o => o.CompanyService.Title));

        }
    }
}
