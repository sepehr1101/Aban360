namespace Aban360.LocationPool.Domain.Features.MainHierarchy.Dto.Queries
{
    public record ReadingBlockGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public int ReadingBoundId { get; set; }
        public string FromReadingNumber { get; set; } = null!;
        public string ToReadingNumber { get; set; } = null!;
    }
}
