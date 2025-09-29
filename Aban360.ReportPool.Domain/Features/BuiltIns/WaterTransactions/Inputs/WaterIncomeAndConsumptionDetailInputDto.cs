namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record WaterIncomeAndConsumptionDetailInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public int? FromConsumption { get; set; }
        public int? ToConsumption { get; set; }

        public double? FromAmount { get; set; }
        public double? ToAmount { get; set; }

        public bool IsNet { get; set; }

        public ICollection<int> ZoneIds { get; set; }
        public ICollection<int> UsageIds { get; set; }
        public ICollection<int> BranchTypeIds { get; set; }

    }
}
