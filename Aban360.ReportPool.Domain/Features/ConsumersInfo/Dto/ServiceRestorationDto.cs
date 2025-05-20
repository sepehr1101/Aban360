namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record ServiceTerminationRestorationDto
    {
        public string BillId { get; set; } = default!;
        public long WaterBillDebt { get; set; }
        public long ServiceLinkDebt { get; set; }
    }
}
