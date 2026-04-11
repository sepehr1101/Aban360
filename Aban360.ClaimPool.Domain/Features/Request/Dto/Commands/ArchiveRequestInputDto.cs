namespace Aban360.ClaimPool.Domain.Features.Request.Dto.Commands
{
    public record ArchiveRequestInputDto
    {
        public int TrackNumber { get; set; }
        public string? Description { get; set; }
        public int? StatusId { get; set; }
        public bool SendToArchive { get; set; }
    }
}
