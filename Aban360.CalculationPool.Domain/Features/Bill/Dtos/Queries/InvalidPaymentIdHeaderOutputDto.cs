using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries
{
    public record InvalidPaymentIdHeaderOutputDto
    {
        public int ZoneId { get; set; }
        public string ZoneTitle { get; set; }
        public int CustomerCount { get; set; }
        public int BillCount { get; set; }
        public int RecordCount { get; set; }
        public string Title { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
    }
}
