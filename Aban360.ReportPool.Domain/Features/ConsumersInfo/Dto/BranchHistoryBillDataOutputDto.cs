namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record BranchHistoryBillDataOutputDto
    {
        public string? LastWaterBillRefundDate { get; set; }
        public string? LastMeterReadingDate { get; set; }
        public string? LastSubscriptionRefundDate { get; set; }
        public string? LastTemporaryDisconnectionDate { get; set; }//

    }
}
