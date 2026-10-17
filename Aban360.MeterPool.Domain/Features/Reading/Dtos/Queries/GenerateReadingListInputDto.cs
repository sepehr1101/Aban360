namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries
{
    public record GenerateReadingListInputDto
    {
        public int ReadingBoundId { get; set; }
        public IEnumerable<PerReadingBlockInfoDto> ReadingBlockInfo { get; set; }
        public int Year { get; set; }
        public int ReadingPeriodTypeId { get; set; }
        public int ReadingPeriodId { get; set; }
    }
}
