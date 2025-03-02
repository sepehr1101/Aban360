namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record ProfessionGetDto
    {
        public short Id { get; set; }
        public short GuildId { get; set; }
        public string GuildTitle { get; set; }
        public string Title { get; set; } = null!;
        public short Description { get; set; }
    }
}
