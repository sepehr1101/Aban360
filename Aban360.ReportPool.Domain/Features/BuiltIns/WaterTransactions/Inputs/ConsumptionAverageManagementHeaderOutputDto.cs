namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record ConsumptionAverageManagementHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public int CustomerCount { get; set; }
        public int RecordCount { get; set; }
        public string ReportDateJalali { get; set; }
        public string Title { get; set; }

        public float ConsumptionAverage { get; set; }
        public float Consumption{ get; set; }

    }
}
