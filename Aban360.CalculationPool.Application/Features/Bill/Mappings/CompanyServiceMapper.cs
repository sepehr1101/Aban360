using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Mappings
{
    public class CompanyServiceMapper : Profile
    {
        public CompanyServiceMapper()
        {
            CreateMap<CompanyServiceCreateDto, CompanyService>();
            CreateMap<CompanyServiceDeleteDto, CompanyService>();
            CreateMap<CompanyServiceUpdateDto, CompanyService>();
            CreateMap<CompanyService, CompanyServiceGetDto>()
                .ForMember(dest=>dest.CompanyServiceTypeTitle,m=>m.MapFrom(o=>o.CompanyServiceType.Title));
        }
    }
}
