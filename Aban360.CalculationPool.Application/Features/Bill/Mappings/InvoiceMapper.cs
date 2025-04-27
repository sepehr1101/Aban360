using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;
using AutoMapper;

namespace Aban360.CalculationPool.Application.Features.Bil.Mappings
{
    public class InvoiceMapper : Profile
    {
        public InvoiceMapper()
        {
            CreateMap<InvoiceCreateDto, Invoice>();
        }
    }
}
