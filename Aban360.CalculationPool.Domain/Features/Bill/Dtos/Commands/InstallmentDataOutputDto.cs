namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record InstallmentDataOutputDto
    {
        public double Amount { get; set; }
        public int InstallmentOrder { get; set; }
        public string DueDateJalali { get; set; }
        public string PaymentId { get; set; }
        public string BillId { get; set; }
    }
}
