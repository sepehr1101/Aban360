namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record GuildUpdateDto
    {
        public short Id { get; set; }
        public short UsageId { get; set; }
        public string Title { get; set; } = null!;
        public short Description { get; set; }
    }
}
