namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record UsageGroup2GetDto
    {
        public short Id { get; set; }
        public string Title { get; set; }
        public int Group1Id { get; set; }
        public string Group1Title { get; set; }
    }
}
