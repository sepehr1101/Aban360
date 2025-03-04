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
            CreateMap<InvoiceLineItemInsertMode, InvoiceLineItemInsertModeCreateDto>().ReverseMap();
            CreateMap<InvoiceLineItemInsertMode, InvoiceLineItemInsertModeDeleteDto>().ReverseMap();
            CreateMap<InvoiceLineItemInsertMode, InvoiceLineItemInsertModeUpdateDto>().ReverseMap();
            CreateMap<InvoiceLineItemInsertMode, InvoiceLineItemInsertModeGetDto>().ReverseMap();
        }
    }
}
