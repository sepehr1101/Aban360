namespace Aban360.BlobPool.Domain.Features.OpenKm
{
    public sealed record CopyTrackImagesToBillInputDto
    {
        public int TrackNumber { get; set; }
        public string BillId { get; set; } = string.Empty;
    }
}
