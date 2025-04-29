namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries
{
    public record InvoiceInstallmentGetDto
    {
        public long Id { get; set; }
        public long InvoiceId { get; set; }
        public long Amount { get; set; }
        public string DueDateJalali { get; set; } = null!;
        public DateTime DueDateTime { get; set; }
        public int InstallmentOrder { get; set; }
        public string? BillId { get; set; }
        public string? PaymentId { get; set; }
    }
}
