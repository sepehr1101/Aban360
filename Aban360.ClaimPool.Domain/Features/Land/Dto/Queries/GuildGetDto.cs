namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record GuildGetDto
    {
        public short Id { get; set; }
        public short UsageId { get; set; }
        public string UsageTitle { get; set; } = default!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}
