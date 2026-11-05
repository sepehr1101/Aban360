using Aban360.Common.Categories.ApiResponse;
using Aban360.SystemPool.Application.Features.Logging.Handlers.Queries.Conracts;
using Aban360.SystemPool.Domain.Features.Logging.Dto.Output;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.SystemPool.Logging.Queries;

[Route("v1/logging/errors/{id:long:min(1)}")]
[Produces("application/json")]
public sealed class HttpErrorLogDetailController(IHttpErrorLogHandler handler, IConfiguration configuration) : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseEnvelope<HttpErrorLogDetailOutput>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(long id, CancellationToken cancellationToken)
    {
        var role = configuration["ApiSettings:AdminRoleName"];
        if (string.IsNullOrWhiteSpace(role) || !User.IsInRole(role)) return Forbid();

        var detail = await handler.GetDetail(id, cancellationToken);
        return detail is null ? NotFound() : Ok(detail);
    }
}
