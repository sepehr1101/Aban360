namespace Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries
{
    public record UsageLevel3GetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short UsageLevel2Id { get; set; }
        public short UsageLevel2Title { get; set; }

    }
}
