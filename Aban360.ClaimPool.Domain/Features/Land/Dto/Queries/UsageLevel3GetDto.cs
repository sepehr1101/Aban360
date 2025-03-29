namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record UsageLevel3GetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short UsageLevel2Id { get; set; }
        public string UsageLevel2Title { get; set; }

    }
}
