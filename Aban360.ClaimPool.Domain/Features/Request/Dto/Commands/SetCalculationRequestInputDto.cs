namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record SetCalculationRequestInputDto
    {
        public int TrackNumber { get; set; }
        public string? Description { get; set; }
    }
}
