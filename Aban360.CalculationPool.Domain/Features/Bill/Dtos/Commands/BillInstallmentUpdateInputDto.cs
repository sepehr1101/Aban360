namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record BillInstallmentUpdateInputDto
    {
        public string BillId { get; set; } = default!;
        public int Id { get; set; }
        public string DeadLineDateJalali { get; set; } = default!;
        public long Amount { get; set; }
    }
}
