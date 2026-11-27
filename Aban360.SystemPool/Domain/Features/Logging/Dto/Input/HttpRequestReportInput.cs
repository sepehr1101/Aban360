using System.ComponentModel.DataAnnotations;

namespace Aban360.SystemPool.Domain.Features.Logging.Dto.Input;

public sealed class HttpRequestReportInput
{
    [Required] public string FromDate { get; set; } = "";
    [Required] public string ToDate { get; set; } = "";
    public string FromTime { get; set; } = "00:00";
    public string ToTime { get; set; } = "00:00";
    public string? UserId { get; set; }
    public string? RouteTemplate { get; set; }
    [Range(1, int.MaxValue)] public int Page { get; set; } = 1;
    [Range(1, 200)] public int PageSize { get; set; } = 50;
}

public enum HttpRequestReportKind { Summary, Routes, Users, Requests }
