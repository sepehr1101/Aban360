using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Mappings
{
    public class InvoiceLineItemInsertModeMapper : Profile
    {
        public InvoiceLineItemInsertModeMapper()
        {
            CreateMap<InvoiceLineItemInsertModeCreateDto,InvoiceLineItemInsertMode>();
            CreateMap<InvoiceLineItemInsertModeDeleteDto,InvoiceLineItemInsertMode>();
            CreateMap<InvoiceLineItemInsertModeUpdateDto,InvoiceLineItemInsertMode>();
            CreateMap<InvoiceLineItemInsertMode, InvoiceLineItemInsertModeGetDto>();
        }
    }
}
