namespace Aban360.CalculationPool.Domain.Features.CollectBills.Outputs
{
    public record CollectBillsErrorOutputDto
    {
        public int Index { get; set; }
        public string Field { get; set; }
        public string Message { get; set; }
        public string Identifier_Bill { get; set; }
        public string Identifier_Pay { get; set; }
    }
}
