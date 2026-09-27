namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record WaterIncomeAndConsumptionSummaryHeaderOutputDto
    {
        public string ReportDateJalali { get; set; }
        public string Title { get; set; }
        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }

        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public double? FromAmount{ get; set; }
        public double? ToAmount{ get; set; }
        public int? FromConsumption{ get; set; }
        public int? ToConsumption{ get; set; }

        public int SumBillCount { get; set; }
        public int SumTransactionCount { get; set; }
        public float SumSewageConsumption { get; set; }
        public int SumConsumption { get; set; }
        public float SumConsumptionAverage { get; set; }
        public int SumBillUnitCounts { get; set; }
        public int SumDuration { get; set; }
        public long SumItems { get; set; }
        public long SumWater { get; set; }
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
        public int BillUnit { get; set; }
        public int TotalUnit { get; set; }
    }
    public record WaterIncomeDiscountSummaryHeaderOutputDto
    {
        public string ReportDateJalali { get; set; }
        public string Title { get; set; }
        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }

        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public double? FromAmount{ get; set; }
        public double? ToAmount{ get; set; }
        public int? FromConsumption{ get; set; }
        public int? ToConsumption{ get; set; }

        public int SumBillCount { get; set; }
        public int SumTransactionCount { get; set; }
        public float SumSewageConsumption { get; set; }
        public int SumConsumption { get; set; }
        public float SumConsumptionAverage { get; set; }
        public int SumBillUnitCounts { get; set; }
        public int SumDuration { get; set; }
        public long SumItemsOff { get; set; }
        public long SumOffWater { get; set; }
        public long SumItemOff1 { get; set; }
        public long SumItemOff2 { get; set; }
        public long SumItemOff3 { get; set; }
        public long SumItemOff4 { get; set; }
        public long SumItemOff5 { get; set; }
        public long SumItemOff6 { get; set; }
        public long SumItemOff7 { get; set; }
        public long SumItemOff8 { get; set; }
        public long SumItemOff9 { get; set; }
        public long SumItemOff10 { get; set; }
        public long SumItemOff11 { get; set; }
        public long SumItemOff12 { get; set; }
        public long SumItemOff13 { get; set; }
        public long SumItemOff14 { get; set; }
        public long SumItemOff15 { get; set; }
        public long SumItemOff16 { get; set; }
        public long SumItemOff17 { get; set; }
        public long SumItemOff18 { get; set; }
        public int BillUnit { get; set; }
        public int TotalUnit { get; set; }
    }
}
