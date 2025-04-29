namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record InvoiceInstallmentCreateDto
    {
        public long Amount { get; set; }
        public string DueDateJalali { get; set; } = null!;
        public DateTime DueDateTime { get; set; }
        public int InstallmentOrder { get; set; }
        public string? BillId { get; set; }
        public string? PaymentId { get; set; }
    }
}
