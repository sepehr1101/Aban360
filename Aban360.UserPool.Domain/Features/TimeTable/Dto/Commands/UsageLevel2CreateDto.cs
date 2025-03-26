namespace Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands
{
    public record UsageLevel2CreateDto
    {
        public string Title { get; set; } = null!;
        public short UsageLevel1Id { get; set; }

    }
}
