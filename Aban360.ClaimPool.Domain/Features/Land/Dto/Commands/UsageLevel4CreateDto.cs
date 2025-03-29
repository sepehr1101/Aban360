namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record UsageLevel4CreateDto
    {
        public string Title { get; set; } = null!;
        public short UsageLevel3Id { get; set; }

    }
}
