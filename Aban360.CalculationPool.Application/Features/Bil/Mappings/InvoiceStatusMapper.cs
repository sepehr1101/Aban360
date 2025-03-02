using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Mappings
{
    public class InvoiceStatusMapper : Profile
    {
        public InvoiceStatusMapper()
        {
            CreateMap<InvoiceStatus, InvoiceStatusCreateDto>().ReverseMap();
            CreateMap<InvoiceStatus, InvoiceStatusDeleteDto>().ReverseMap();
            CreateMap<InvoiceStatus, InvoiceStatusUpdateDto>().ReverseMap();
            CreateMap<InvoiceStatus, InvoiceStatusGetDto>().ReverseMap();
        }
    }
}
