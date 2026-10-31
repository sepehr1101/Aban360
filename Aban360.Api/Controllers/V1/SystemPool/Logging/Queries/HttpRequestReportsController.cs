using Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Conracts;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.SystemPool.Logging.Queries;

[Route("v1/logging/http-requests")]
public sealed class HttpRequestReportsController(IHttpRequestReportHandler handler, IConfiguration configuration) : BaseController
{
    [HttpGet("summary")]
    public Task<IActionResult> Summary([FromQuery] HttpRequestReportInput input, CancellationToken cancellationToken)
        => Report(input, HttpRequestReportKind.Summary, cancellationToken);

    [HttpGet("routes")]
    public Task<IActionResult> Routes([FromQuery] HttpRequestReportInput input, CancellationToken cancellationToken)
        => Report(input, HttpRequestReportKind.Routes, cancellationToken);

    [HttpGet("users")]
    public Task<IActionResult> Users([FromQuery] HttpRequestReportInput input, CancellationToken cancellationToken)
        => Report(input, HttpRequestReportKind.Users, cancellationToken);

    [HttpGet("slowest")]
    public Task<IActionResult> Slowest([FromQuery] HttpRequestReportInput input, CancellationToken cancellationToken)
        => Report(input, HttpRequestReportKind.Requests, cancellationToken);

    private async Task<IActionResult> Report(HttpRequestReportInput input, HttpRequestReportKind kind, CancellationToken cancellationToken)
    {
        var role = configuration["ApiSettings:AdminRoleName"];
        if (string.IsNullOrWhiteSpace(role) || !User.IsInRole(role)) return Forbid();
        return Ok(await handler.Handle(input, kind, cancellationToken));
    }
}
