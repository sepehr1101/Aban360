namespace Aban360.CalculationPool.Domain.Features.MeterReading.Dtos.Queries
{
    public record MeterReadingUploadToRegisteredSummaryDataOutputDto
    {
        public string ZoneTitle { get; set; }
        public int FirstFlowId { get; set; }
        public string FileName { get; set; }
        public int FileCount { get; set; }
        public int AllRecords { get; set; }
        public int CustomerCount { get; set; }
        public int RemovedAndUpdatedCount { get; set; }
        public int ExcludedCount { get; set; }
        public int DistanceMax { get; set; }
        public int DistanceMin { get; set; }
        public float DistanceAverage { get; set; }
        public int DistanceMedian { get; set; }

    }
}
