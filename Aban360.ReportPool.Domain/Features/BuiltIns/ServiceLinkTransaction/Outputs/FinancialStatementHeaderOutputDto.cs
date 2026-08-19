using DNTPersianUtils.Core;

namespace Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs
{
    public record FinancialStatementHeaderOutputDto
    {
        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public string Title { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public int RecordCount { get; set; }

        public int CustomerCount { get; set; }
        public int ConsumptionTotalUnit { get; set; }
        public int DailyAverage { get; set; }
        public int NetConsumption { get; set; }
        public long NetAmount { get; set; }
        public int ReturnedConsumption { get; set; }
        public long ReturnedAmount { get; set; }
        public long DiscountAmount { get; set; }
        public int RawConsumption { get; set; }
        public long RawAmount { get; set; }
        public float RawAmountAverage { get; set; }
        public float ConsumptionAverageInMonth { get; set; }
    }
}
