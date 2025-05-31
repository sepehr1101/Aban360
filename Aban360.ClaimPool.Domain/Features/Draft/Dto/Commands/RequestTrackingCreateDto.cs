namespace Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands
{
    public record RequestTrackingCreateDto
    {
        public int WaterMeterId { get; set; }
        public short Status { get; set; }
    }}
