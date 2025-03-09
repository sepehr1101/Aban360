using Aban360.CalculationPool.Domain.Constants;

namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record InvoiceLineItemInsertModeCreateDto
    {
        public InvoiceLineItemInsertModeEnum Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}
