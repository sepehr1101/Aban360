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
        
        public long CustomerCount { get; set; }
        public long ConsumptionTotalUnit { get; set; }
        public decimal DailyAverage { get; set; }
        public long NetConsumption { get; set; }
        public long NetAmount { get; set; }
        public long ReturnedConsumption { get; set; }
        public long ReturnedAmount { get; set; }
        public long DiscountAmount { get; set; }
        public long RawConsumption { get; set; }
        public long RawAmount { get; set; }
        public decimal RawAmountAverage { get; set; }
        public decimal ConsumptionAverageInMonth { get; set; }
    }
}
