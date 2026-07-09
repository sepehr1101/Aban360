namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record UsageGroup3UpdateDto
    {
        public short Id { get; set; }
        public int Group2Id { get; set; }
        public int UsageId { get; set; }
    }
}
