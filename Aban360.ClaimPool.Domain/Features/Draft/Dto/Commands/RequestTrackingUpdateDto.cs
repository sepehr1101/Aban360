namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record RequestTrackingUpdateDto
    {
        public int Id { get; set; }
        public int WaterMeterId { get; set; }
        public short Status { get; set; }
    }
}
