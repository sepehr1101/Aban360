namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record OtherExpensesItemsGetDto
    {
        public int ZoneId { get; set; }
        public int UsageId { get; set; }
        public int ServiceId { get; set; }
        public OtherExpensesItemsGetDto(int zoneId, int usageId, int seviceId)
        {
            ZoneId = zoneId;
            UsageId = usageId;
            ServiceId = seviceId;
        }
        public OtherExpensesItemsGetDto()
        {
        }
    }
}
