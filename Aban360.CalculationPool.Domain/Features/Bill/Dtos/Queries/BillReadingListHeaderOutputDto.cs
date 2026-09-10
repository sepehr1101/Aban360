using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries
{
    public record BillReadingListHeaderOutputDto
    {
        public string  ZoneTitle { get; set; }
        public int ZoneId { get; set; }
        public string FromReadingNumber { get; set; }
        public string ToReadingNumber { get; set; }

        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
    }
}
