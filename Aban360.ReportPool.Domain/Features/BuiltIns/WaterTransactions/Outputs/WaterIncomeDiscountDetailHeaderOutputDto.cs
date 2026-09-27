namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs
{
    public record WaterIncomeDiscountDetailHeaderOutputDto
    {
        public string Title { get; set; }
        public string ReportDateJalali { get; set; }
        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }

        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public double? FromAmount { get; set; }
        public double? ToAmount { get; set; }
        public int? FromConsumption { get; set; }
        public int? ToConsumption { get; set; }

        public int SumBillCount { get; set; }
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
