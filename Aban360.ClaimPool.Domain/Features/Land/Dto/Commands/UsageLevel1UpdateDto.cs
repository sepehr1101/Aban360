namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record UsageLevel1UpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
