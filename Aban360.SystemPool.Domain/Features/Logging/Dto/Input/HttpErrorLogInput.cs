using System.ComponentModel.DataAnnotations;

namespace Aban360.SystemPool.Domain.Features.Logging.Dto.Input;

public sealed class HttpErrorLogInput
{
    [Required] public string StatusCodeClass { get; set; } = "";
    [Required] public string FromDate { get; set; } = "";
    [Required] public string ToDate { get; set; } = "";
    [Required] public string FromTime { get; set; } = "";
    [Required] public string ToTime { get; set; } = "";
    [Range(1, int.MaxValue)] public int Page { get; set; } = 1;
    [Range(1, 200)] public int PageSize { get; set; } = 50;
}
