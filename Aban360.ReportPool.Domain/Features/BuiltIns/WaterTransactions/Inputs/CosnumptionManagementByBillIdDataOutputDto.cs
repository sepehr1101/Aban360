namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record CosnumptionManagementByBillIdDataOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public int Consumption { get; set; }
        public int Duration { get; set; }
        public CosnumptionManagementByBillIdDataOutputDto(string fromDateJalali, string toDateJalali, int consumption, int duration)
        {
            FromDateJalali = fromDateJalali;
            ToDateJalali = toDateJalali;
            Consumption = consumption;
            Duration = duration;
        }
        public CosnumptionManagementByBillIdDataOutputDto()
        {
        }
    }
}
