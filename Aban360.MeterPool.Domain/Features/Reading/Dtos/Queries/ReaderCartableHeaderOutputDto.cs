using DNTPersianUtils.Core;

namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries
{
    public record ReaderCartableHeaderOutputDto
    {
        public Guid ReaderId { get; set; }
        public string ReaderName { get; set; }
        public string ReaderPhoneNumber { get; set; }
        public int RecordCount { get; set; }
        public int CustomerCount { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
    }
}
