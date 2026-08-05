namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ConsumptionManagementByBillIdDto
    {
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public ConsumptionManagementByBillIdDto(int zoneId, int customerNumber, string fromDateJalali, string toDateJalali)
        {
            ZoneId = zoneId;
            CustomerNumber = customerNumber;
            FromDateJalali = fromDateJalali;
            ToDateJalali = toDateJalali;
        }
        public ConsumptionManagementByBillIdDto()
        {
        }
    }
}
