namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ConsumptionManagementByBillIdGetDto
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public int CustomerNumber { get; set; }
        public string BillId { get; set; }
        public string ReadingNumber { get; set; }
        public string PreviousDateJalali { get; set; }
        public int PreviousNumber { get; set; }
        public string CurrentDateJalali { get; set; }
        public int CurrentNumber { get; set; }
        public int Duration { get; set; }
        public int Consumption { get; set; }
        public int ConsumptionAverage { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }

    }
}
