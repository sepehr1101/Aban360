using DNTPersianUtils.Core;

namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries
{
    public record DetailHeaderDto
    {
        public int RecordCount { get; set; }
        public string Title { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
    }
}
