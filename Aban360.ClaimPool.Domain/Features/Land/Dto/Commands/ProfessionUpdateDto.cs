namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record ProfessionUpdateDto
    {
        public short Id { get; set; }
        public short GuildId { get; set; }
        public string Title { get; set; } = null!;
        public short Description { get; set; }
    }
}
