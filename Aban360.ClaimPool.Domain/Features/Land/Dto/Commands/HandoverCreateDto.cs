namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Commands
{
    public record HandoverCreateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
