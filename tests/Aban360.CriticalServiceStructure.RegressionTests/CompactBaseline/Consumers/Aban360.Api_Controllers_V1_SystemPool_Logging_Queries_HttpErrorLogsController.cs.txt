using Aban360.Common.Categories.ApiResponse;
using Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Conracts;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Input;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.SystemPool.Logging.Queries;

[Route("v1/logging/errors")]
public sealed class HttpErrorLogsController(IHttpErrorLogHandler handler, IConfiguration configuration) : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseEnvelope<IEnumerable<HttpErrorLogOutput>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromQuery] HttpErrorLogInput input, CancellationToken cancellationToken)
    {
        var role = configuration["ApiSettings:AdminRoleName"];
        if (string.IsNullOrWhiteSpace(role) || !User.IsInRole(role)) return Forbid();
        return Ok(await handler.Handle(input, cancellationToken));
    }
}
