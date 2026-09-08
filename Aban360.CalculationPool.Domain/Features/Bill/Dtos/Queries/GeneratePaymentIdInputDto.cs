namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries
{
    public record GeneratePaymentIdInputDto
    {
        public string BillId { get; set; }
        public long Amount { get; set; }
        public bool IsWater { get; set; }
    }
}
