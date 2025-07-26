namespace Aban360.Api.Cronjobs
{
    public class ReportJobArgs
    {
        public string? ReportInputJson { get; set; } // JSON serialized input
        public string ReportInputType { get; set; } = default!; // Full type name
        public string HeadType { get; set; } = default!;        // Full type name
        public string DataType { get; set; } = default!;        // Full type name
        public string UserId { get; set; } = default!;
        public string ReportTitle { get; set; }=default!
        public string? ConnectionId { get; set; }
    }
}
