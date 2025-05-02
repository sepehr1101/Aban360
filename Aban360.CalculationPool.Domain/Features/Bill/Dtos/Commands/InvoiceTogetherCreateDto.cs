namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record InvoiceTogetherCreateDto
    {
        public string? BillId { get; set; }
        public string? PaymentId { get; set; }
        public int PrepaymentPercent { get; set; }
        public int InstallmentCount { get; set; }
        public short PaymentPeriod { get; set; }
        public InvoiceCreateDto Invoice { get; set; }
        public ICollection<InvoiceLineItemCreateDto> InvoiceLineItem { get; set; }
    }
}
