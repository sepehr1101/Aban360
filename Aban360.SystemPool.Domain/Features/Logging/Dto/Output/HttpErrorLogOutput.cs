namespace Aban360.SystemPool.Domain.Features.Logging.Dto.Output;

public record HttpErrorLogOutput
{
    public long Id { get; set; }
    public string DateJalali { get; set; } = "";
    public string Time { get; set; } = "";
    public int StatusCode { get; set; }
    public string? RequestMethod { get; set; }
    public string? RouteTemplate { get; set; }
    public string? TraceId { get; set; }
    public string? UserId { get; set; }
    public string? Message { get; set; }
}
