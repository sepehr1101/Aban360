namespace Aban360.MeterPool.Domain.Features.AutoReading.Dtos.Queries
{
    public sealed class AutoReadingOptions
    {
        public const string SectionName = "AutoReading";
        public string BaseUrl { get; set; } = default!;
        public string FlowMeterReportEndpoint { get; set; } = default!;
    }
}
