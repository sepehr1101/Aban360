namespace Aban360.BlobPool.Domain.Features.OpenKm
{
    public record RemoveFileDto
    {
        public string FolderName { get; set; } = default!;
        public bool IsBillId { get; set; }
        public string Uuid { get; set; } = default!;
    }
}
