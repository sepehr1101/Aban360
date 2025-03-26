namespace Aban360.UserPool.Domain.Features.TimeTable.Dto.Queries
{
    public record UsageLevel2GetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short UsageLevel1Id { get; set; }
        public short UsageLevel1Title { get; set; }

    }
}
