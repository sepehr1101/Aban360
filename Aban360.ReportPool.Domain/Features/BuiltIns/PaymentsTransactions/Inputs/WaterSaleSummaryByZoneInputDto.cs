using Aban360.ReportPool.Domain.Constants;

namespace Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs
{
    public record WaterSaleSummaryByZoneInputDto
    {
        public ICollection<int> ZoneIds { get; set; }
        public ICollection<int> UsageIds { get; set; }
        public ICollection<int> BranchTypeIds { get; set; }

        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }

        public bool HasUsageGroup { get; set; }
        public bool IsNet { get; set; }
        public WaterSaleGroupedInputEnum InputGrouped { get; set; }
        public int? CounterStateId { get; set; }
    }
}
