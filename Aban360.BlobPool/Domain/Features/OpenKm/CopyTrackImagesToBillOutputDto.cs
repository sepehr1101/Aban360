namespace Aban360.BlobPool.Domain.Features.OpenKm
{
    public sealed class CopyTrackImagesToBillOutputDto
    {
        public int TrackNumber { get; init; }
        public string BillId { get; init; } = string.Empty;
        public bool IsLinked { get; init; }
        public bool IsComplete => Files.All(file => file.Status is "Copied" or "Skipped");
        public int CopiedCount => Files.Count(file => file.Status == "Copied");
        public int SkippedCount => Files.Count(file => file.Status == "Skipped");
        public int ConflictCount => Files.Count(file => file.Status == "Conflict");
        public int FailedCount => Files.Count(file => file.Status == "Failed");
        public List<CopyTrackImageFileOutputDto> Files { get; } = new();
        public List<CopyTrackImagesToBillWarningDto> Warnings { get; } = new();
    }

    public sealed class CopyTrackImagesToBillWarningDto
    {
        public string Code { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
    }

    public sealed class CopyTrackImageFileOutputDto
    {
        public string SourceUuid { get; init; } = string.Empty;
        public string FileName { get; init; } = string.Empty;
        public string? DestinationUuid { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ErrorCode { get; set; }
        public string? Message { get; set; }
    }
}
