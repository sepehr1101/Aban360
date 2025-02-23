namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public record SiphonSummaryDto
    {
        public string? InstallationLocation { get; set; }     
        public string InstallationDate { get; set; }     
        public string SiphonTypeTitle { get; set; }     
        public string SiphonDiameterTitle { get; set; }     
    }
}
