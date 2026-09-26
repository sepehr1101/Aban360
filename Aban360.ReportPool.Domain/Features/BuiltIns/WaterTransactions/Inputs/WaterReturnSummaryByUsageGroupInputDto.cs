using Aban360.ReportPool.Domain.Constants;

namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record WaterReturnSummaryByUsageGroupInputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public int? FromConsumption { get; set; }
        public int? ToConsumption { get; set; }

        public double? FromAmount { get; set; }
        public double? ToAmount { get; set; }

        public WaterIncomeAndConsumptionTypeEnum type { get; set; }

        public int UsageGroupId { get; set; }
        public ICollection<int> ZoneIds { get; set; }
        public ICollection<int> BranchTypeIds { get; set; }
        public ICollection<int> ReturnCauseIds { get; set; }

        public WaterReturnSummaryEnum EnumInput { get; set; }
    }
}
