namespace Aban360.ClaimPool.Domain.Features.Land.Dto.Queries
{
    public record ConnectDisconnectRemoveInputDto
    {
        public long Id { get; set; }
        public string? Description { get; set; }
    }
}
