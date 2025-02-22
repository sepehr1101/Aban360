namespace Aban360.ReportPool.Persistence.Queries.Implementations
{
    public record IndividualSummaryDto
    {
        public string FullName { get; set; }
        public string? FatherName { get; set; }
        public string? NationalId { get; set; }
        public string? PhoneNumbers { get; set; }
        public string? MobileNumbers { get; set; }
    }
}
