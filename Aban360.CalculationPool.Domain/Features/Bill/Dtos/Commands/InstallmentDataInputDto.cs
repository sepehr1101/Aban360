namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record InstallmentDataInputDto
    {
        public string DeadLineDateJalali { get; set; } = default!;
        public long Amount { get; set; }
    }
}
