using DNTPersianUtils.Core;

namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries
{
    public record MeterReadingCartableHeaderOutputDto
    {
        public string? FromReadingNumber { get; set; }
        public string? ToReadingNumber { get; set; }
        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }
        public string Title { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
    }
}
