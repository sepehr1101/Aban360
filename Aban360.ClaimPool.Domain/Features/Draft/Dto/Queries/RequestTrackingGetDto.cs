namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Queries
{
    public record RequestTrackingGetDto
    {
        public int Id { get; set; }
        public int WaterMeterId { get; set; }
        public short Status { get; set; }
    }
}