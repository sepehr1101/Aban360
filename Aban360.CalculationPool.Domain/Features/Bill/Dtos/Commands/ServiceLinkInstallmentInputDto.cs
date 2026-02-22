namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record ServiceLinkInstallmentInputDto
    {
        public string BillId { get; set; }
        public int InstallmentCount { get; set; }
        public bool IsConfirm { get; set; }
        public int AdvancePaymentPercentage { get; set; }
    }
}
