namespace Aban360.BlobPool.Domain.Features.DmsServices.Dto.Commands
{
    public record AddBase64FileInput
    {
        public string File { get; set; } = default!;
        public string FileName { get; set; } = default!;
        public int DocumentTypeId { get; set; }
        public string? BillId { get; set; }
        public long? TrackNumber { get; set; }
    }
}
