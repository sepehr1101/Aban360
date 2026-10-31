namespace Aban360.SystemPool.Domain.Features.Logging.Dto.Output;

public sealed class HttpRequestReportOutput
{
    public long TotalGroups { get; set; }
    public string? RequestMethod { get; set; }
    public string? RouteTemplate { get; set; }
    public string? UserId { get; set; }
    public string? TraceId { get; set; }
    public DateTime? TimeStamp { get; set; }
    public int? StatusCode { get; set; }
    public long RequestCount { get; set; }
    public long ServerErrorCount { get; set; }
    public double AverageMilliseconds { get; set; }
    public double MaximumMilliseconds { get; set; }
    public double TotalMilliseconds { get; set; }
}
