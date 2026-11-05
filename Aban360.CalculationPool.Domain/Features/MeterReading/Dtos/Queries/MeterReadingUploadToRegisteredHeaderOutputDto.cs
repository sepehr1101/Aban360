using DNTPersianUtils.Core;

namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterReadingUploadToRegisteredHeaderOutputDto
    {
        public int FileCount { get; set; }
        public int AllRecords { get; set; }
        public int CustomerCount { get; set; }
        public int RemovedAndUpdatedCount { get; set; }
        public int ExcludedCount { get; set; }
        public int DistanceMax { get; set; }
        public int DistanceMin { get; set; }
        public float DistanceAverage { get; set; }
        public float DistanceMedian { get; set; }

        public string FromDateJalali { get; set; }
        public string ToDateJalali { get; set; }
        public string Title { get; set; }
        public string ReportDateJalali { get; set; } = DateTime.Now.ToShortPersianDateString();
    }
}
