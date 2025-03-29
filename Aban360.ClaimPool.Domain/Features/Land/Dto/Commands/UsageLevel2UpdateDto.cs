namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record UsageLevel2UpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short UsageLevel1Id { get; set; }

    }
}
