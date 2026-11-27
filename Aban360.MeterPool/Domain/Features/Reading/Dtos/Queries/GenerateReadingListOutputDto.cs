namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries
{
    public record GenerateReadingListOutputDto
    {
        public string ZoneTitle { get; set; }
        public int BlockCount { get; set; }
        public int Year { get; set; }
        public string ReadingPeriodTypeTitle { get; set; }
        public string ReadingPeriodTitle { get; set; }
        public int CustomerCount { get; set; }
    }
}
