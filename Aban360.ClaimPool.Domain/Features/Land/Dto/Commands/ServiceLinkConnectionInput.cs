namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record ServiceLinkConnectionInput
    {
        public long Id { get; set; }
        public string? Description { get; set; }
        public string? Why { get; set; }
    }
}
