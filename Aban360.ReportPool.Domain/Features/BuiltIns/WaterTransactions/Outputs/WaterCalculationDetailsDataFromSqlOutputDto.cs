namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record WaterCalculationDetailsDataFromSqlOutputDto
    {
        public string ItemTitle { get; set; }
        public long ItemValue { get; set; }
        public long SumItems { get; set; }
        public long SumOffItems { get; set; }
        public long Payble { get; set; }
    }
}
