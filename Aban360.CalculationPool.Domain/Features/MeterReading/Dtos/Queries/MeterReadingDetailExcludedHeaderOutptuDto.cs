using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterReadingDetailExcludedHeaderOutptuDto
    {
        public string FromExcludeDateJalali { get; set; }
        public string ToExcludeDateJalali { get; set; }
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }

        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
    }
}
