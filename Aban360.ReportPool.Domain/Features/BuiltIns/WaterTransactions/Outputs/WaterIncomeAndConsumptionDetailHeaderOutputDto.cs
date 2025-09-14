namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record WaterIncomeAndConsumptionDetailHeaderOutputDto
    {
        public string Title { get; set; }
        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }

        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public double? FromAmount{ get; set; }
        public double? ToAmount{ get; set; }
        public int? FromConsumption{ get; set; }
        public int? ToConsumption{ get; set; }

        public int SumBillCount { get; set; }
        public int SumConsumption { get; set; }
        public float SumConsumptionAverage { get; set; }
        public int SumBillUnitCounts { get; set; }
        public int SumDuration { get; set; }
        public long SumItems { get; set; }
        public long SumItem1 { get; set; }
        public long SumItem2 { get; set; }
        public long SumItem3 { get; set; }
        public long SumItem4 { get; set; }
        public long SumItem5 { get; set; }
        public long SumItem6 { get; set; }
        public long SumItem7 { get; set; }
        public long SumItem8 { get; set; }
        public long SumItem9 { get; set; }
        public long SumItem10 { get; set; }
        public long SumItem11 { get; set; }
        public long SumItem12 { get; set; }
        public long SumItem13 { get; set; }
        public long SumItem14 { get; set; }
        public long SumItem15 { get; set; }
        public long SumItem16 { get; set; }
        public long SumItem17 { get; set; }
        public long SumItem18 { get; set; }
    }
}
