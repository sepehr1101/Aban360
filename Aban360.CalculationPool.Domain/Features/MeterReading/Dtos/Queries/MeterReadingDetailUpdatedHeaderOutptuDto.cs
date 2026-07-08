using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterReadingDetailUpdatedHeaderOutptuDto
    {
        public int RecordCount { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
        public string Title { get; set; }
        public MeterReadingDetailUpdatedHeaderOutptuDto(int recordCount, string title)
        {
            RecordCount = recordCount;
            Title = title;
        }
        public MeterReadingDetailUpdatedHeaderOutptuDto()
        {
        }
    }
}
