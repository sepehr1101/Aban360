using Aban360.ReportPool.Domain.Constants;

namespace Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs
{
    public record WaterDiscountSummaryInputDto
    {
        public IEnumerable<int> ZoneIds { get; set; }
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public WaterDiscountGroupInputEnum GroupType { get; set; }
    }
}
