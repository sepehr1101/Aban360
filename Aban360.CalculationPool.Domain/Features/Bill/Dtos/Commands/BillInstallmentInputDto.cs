namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record BillInstallmentInputDto
    {
        public string BillId { get; set; }
        public int InstallmentCount { get; set; }
        public bool IsConfirm { get; set; }
    }
}
