namespace Aban360.SystemPool.Domain.Features.Logging.Dto.Output;

public sealed record HttpErrorLogDetailOutput : HttpErrorLogOutput
{
    public DateTime TimeStamp { get; set; }
    public string? Level { get; set; }
    public double? ElapsedMilliseconds { get; set; }
    public string? Exception { get; set; }
}
