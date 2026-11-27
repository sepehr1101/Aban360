namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Queries
{
    public record PerReadingBlockInfoDto
    {
        public int ReadingBlockId { get; set; }
        public int ReaderCode { get; set; }
    }
}
