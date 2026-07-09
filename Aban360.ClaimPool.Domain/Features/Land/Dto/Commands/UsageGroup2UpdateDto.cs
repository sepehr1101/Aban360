namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record UsageGroup2UpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; }
        public int Group1Id { get; set; }
    }
}
