namespace Aban360.LocationPool.Domain.Features.MainHirearchy.Dto.Commands
{
    public record ReadingBoundCreateDto
    {
        public string Title { get; set; } = null!;
        public int ZoneId { get; set; }
        public string FromReadingNumber { get; set; } = null!;
        public string ToReadingNumber { get; set; } = null!;
    }
}
