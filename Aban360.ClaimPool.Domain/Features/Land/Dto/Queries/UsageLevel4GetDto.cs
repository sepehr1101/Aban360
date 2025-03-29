namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record UsageLevel4GetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short UsageLevel3Id { get; set; }
        public string UsageLevel3Title { get; set; }

    }
}
