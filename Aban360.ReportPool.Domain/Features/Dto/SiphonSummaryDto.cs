namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public record SiphonSummaryDto
    {
        public int Id { get; set; }
        public string? InstallationLocation { get; set; }
        public string InstallationDate { get; set; } = default!;
        public string SiphonTypeTitle { get; set; } = default!;
        public string SiphonDiameterTitle { get; set; } = default!;
    }
}
