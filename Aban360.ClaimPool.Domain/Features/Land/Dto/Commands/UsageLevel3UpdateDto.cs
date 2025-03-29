namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record UsageLevel3UpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short UsageLevel2Id { get; set; }

    }
}
