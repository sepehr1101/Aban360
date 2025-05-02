using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record InvoiceLineItemCreateDto
    {
        public short OfferingId { get; set; }
        public long Amount { get; set; }
        public int Quanity { get; set; }
    }
}
