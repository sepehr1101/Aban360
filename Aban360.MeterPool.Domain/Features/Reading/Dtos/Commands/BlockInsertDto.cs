namespace Aban360.MeterPool.Domain.Features.Reading.Dtos.Commands
{
    public record BlockInsertDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid BoundId { get; set; }
        public int ReadingBlockId { get; set; }
        public string ReadingBlockTitle { get; set; } = null!;
        public string FromReadingNumber { get; set; } = null!;
        public string ToReadingNumber { get; set; } = null!;
        public int CustomerCount { get; set; }
        public int OrderDigit { get; set; }
        public string OrderPersian { get; set; } = null!;
    }
}
