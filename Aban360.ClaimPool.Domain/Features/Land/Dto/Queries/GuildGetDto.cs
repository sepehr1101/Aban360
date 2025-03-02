namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record GuildGetDto
    {
        public short Id { get; set; }
        public short UsageId { get; set; }
        public string UsageTitle { get; set; }
        public string Title { get; set; } = null!;
        public short Description { get; set; }
    }
}
