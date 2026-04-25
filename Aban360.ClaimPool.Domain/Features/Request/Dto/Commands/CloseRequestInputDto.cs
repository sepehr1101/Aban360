namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record CloseRequestInputDto
    {
        public int TrackNumber { get; set; }
        public int ZoneId { get; set; }
        public int Id { get; set; }
    }
}
