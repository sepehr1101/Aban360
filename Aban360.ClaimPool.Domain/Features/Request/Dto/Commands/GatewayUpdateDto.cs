namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record GatewayUpdateDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
