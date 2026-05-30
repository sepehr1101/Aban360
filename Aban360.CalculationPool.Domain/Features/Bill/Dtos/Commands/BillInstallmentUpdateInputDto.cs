namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record BillInstallmentUpdateInputDto
    {
        public string  BillId { get; set; }
        public int Id { get; set; }
        public string DueDateJalali { get; set; }
        public long Amount { get; set; }
    }
}
