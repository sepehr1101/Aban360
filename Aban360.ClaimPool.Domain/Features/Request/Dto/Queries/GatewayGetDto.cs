namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record GatewayGetDto
    {
        public short Id { get; set; }
        public string Title { get; set; } = null!;
    }
}
