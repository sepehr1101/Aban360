namespace Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries
{
    public record UsageLevel4GetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short UsageLevel3Id { get; set; }
        public short UsageLevel3Title { get; set; }

    }
}
