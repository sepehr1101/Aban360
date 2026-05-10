namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record BillInstallmentManualInputDto
    {
        public string BillId { get; set; }
        public bool IsConfirm { get; set; }
        public ICollection<InstallmentDataInputDto> Installments { get; set; }
    }
}
