namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record UsageCreateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
        public short ProvienceId { get; set; }
    }
}
