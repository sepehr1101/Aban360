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
            CreateMap<CompanyServiceTypeCreateDto, CompanyServiceType>().ReverseMap();
            CreateMap<CompanyServiceTypeDeleteDto, CompanyServiceType>().ReverseMap();
            CreateMap<CompanyServiceTypeUpdateDto, CompanyServiceType>().ReverseMap();
            CreateMap<CompanyServiceTypeGetDto, CompanyServiceType>().ReverseMap();
        }
    }
}
