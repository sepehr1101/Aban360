namespace Aban360.CalculationPool.Domain.Features.ServiceLink
{
    public record OtherExpensesItemsInsertDto
    {
        public int ServiceId { get; set; }
        public string ServiceTitle { get; set; }
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
        public long Amount { get; set; }
        public Guid InsertBy { get; set; }
        public DateTime InsertDateTime { get; set; }

    }
}
