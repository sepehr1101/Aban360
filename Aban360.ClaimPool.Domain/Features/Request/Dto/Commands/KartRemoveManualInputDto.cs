namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record KartRemoveManualInputDto
    {
        public int Id { get; set; }
        public int TrackNumber { get; set; }
    }
}
