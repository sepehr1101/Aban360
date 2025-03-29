namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record UsageLevel2GetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short UsageLevel1Id { get; set; }
        public string UsageLevel1Title { get; set; }

    }
}
