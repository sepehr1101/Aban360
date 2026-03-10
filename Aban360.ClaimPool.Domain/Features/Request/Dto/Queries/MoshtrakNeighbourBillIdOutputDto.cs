namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Queries
{
    public record MoshtrakNeighbourBillIdOutputDto
    {
        public int TrackNumber { get; set; }
        public string NeighbourBillId { get; set; }
    }
}