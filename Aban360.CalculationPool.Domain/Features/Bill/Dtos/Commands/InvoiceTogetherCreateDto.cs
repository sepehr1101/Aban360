namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record InvoiceTogetherCreateDto
    {
        public string? BillId { get; set; }
        public string? PaymentId { get; set; }
        public InvoiceCreateDto Invoice { get; set; }
        public ICollection<InvoiceLineItemCreateDto> InvoiceLineItem { get; set; }

    }
}
