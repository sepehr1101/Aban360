namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record InvoiceLineItemInsertModeCreateDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}
