namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record UsageGroup3InsertDto
    {
        public short Group2Id { get; set; }
        public IEnumerable<int> UsageIds { get; set; }
    }
}
