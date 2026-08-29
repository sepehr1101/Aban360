namespace Aban360.ReportPool.Domain.Features.ConsumersInfo.Dto
{
    public record SubscriptionCardexInputDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string? FromDate { get; set; }
        public bool HasRemovedBills { get; set; }
        public SubscriptionCardexInputDto(int zoneId, int customerNumber, string? fromDate, bool hasRemovedBills)
        {
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            FromDate = fromDate;
            HasRemovedBills = hasRemovedBills;
        }
        public SubscriptionCardexInputDto()
        {
        }
    }
}
