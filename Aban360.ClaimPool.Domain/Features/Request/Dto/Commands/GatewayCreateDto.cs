namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record GatewayCreateDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
