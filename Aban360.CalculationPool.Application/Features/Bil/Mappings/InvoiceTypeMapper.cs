using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Mappings
{
    public class InvoiceTypeMapper : Profile
    {
        public InvoiceTypeMapper()
        {
            CreateMap<InvoiceType, InvoiceTypeCreateDto>().ReverseMap();
            CreateMap<InvoiceType, InvoiceTypeDeleteDto>().ReverseMap();
            CreateMap<InvoiceType, InvoiceTypeUpdateDto>().ReverseMap();
            CreateMap<InvoiceType, InvoiceTypeGetDto>().ReverseMap();
        }
    }
}
