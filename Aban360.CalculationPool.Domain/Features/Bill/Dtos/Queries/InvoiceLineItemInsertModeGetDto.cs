using Aban360.CalculationPool.Domain.Constants;
using Aban360.CalculationPool.Domain.Features.Bill.Entities;

namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries
{
    public record InvoiceLineItemInsertModeGetDto
    {
        public InvoiceLineItemInsertModeEnum Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}
