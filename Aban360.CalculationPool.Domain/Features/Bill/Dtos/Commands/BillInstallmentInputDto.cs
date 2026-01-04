namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands
{
    public record BillInstallmentInputDto
    {
        public int Id { get; set; }
        public string BillId { get; set; }
        public int InstallmentCount { get; set; }
    }
}
