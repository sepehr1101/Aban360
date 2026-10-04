namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record OtherExpensesItemsGetDto
    {
        public int ZoneId { get; set; }
        public int UsageId { get; set; }
        public int ItemId { get; set; }
        public OtherExpensesItemsGetDto(int zoneId,int usageId,int itemId)
        {
            ZoneId=zoneId;
            UsageId=usageId;
            ItemId=itemId;
        }
        public OtherExpensesItemsGetDto()
        {
        }
    }
}
