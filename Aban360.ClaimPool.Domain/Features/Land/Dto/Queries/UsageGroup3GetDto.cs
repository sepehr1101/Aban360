namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record UsageGroup3GetDto
    {
        public short Id { get; set; }
        public short Group2Id { get; set; }
        public string Group2Title { get; set; }
        public short Group1Id { get; set; }
        public string Group1Title { get; set; }
        public int UsageId { get; set; }
        public string UsageTitle { get; set; }
    }
}
