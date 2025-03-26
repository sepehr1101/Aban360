namespace Aban360.UserPool.Domain.Features.TimeTable.Dto.Commands
{
    public record UsageLevel1UpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
