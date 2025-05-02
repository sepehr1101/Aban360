using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Mappings
{
    public class InvoiceLineItemMapper : Profile
    {
        public InvoiceLineItemMapper()
        {
            CreateMap<InvoiceLineItemCreateDto, InvoiceLineItem>();
        }
    }
}
