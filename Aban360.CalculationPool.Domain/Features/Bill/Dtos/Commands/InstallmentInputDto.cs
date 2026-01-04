namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record InstallmentInputDto
    {
        public string TrackNumber { get; set; }
        public string BillId { get; set; }
        public int InstallmentCount { get; set; }
        public float AdvancePaymentPercentage { get; set; }
    }
}
