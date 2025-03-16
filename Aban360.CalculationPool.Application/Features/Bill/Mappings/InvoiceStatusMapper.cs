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
            CreateMap<InvoiceStatusCreateDto, InvoiceStatus>();
            CreateMap<InvoiceStatusDeleteDto,InvoiceStatus >();
            CreateMap<InvoiceStatusUpdateDto,InvoiceStatus>();
            CreateMap<InvoiceStatus, InvoiceStatusGetDto>();
        }
    }
}
