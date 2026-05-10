namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record InstallmentDataInputDto
    {
        public string DueDateJalali { get; set; }
        public long Amount { get; set; }
    }
}
