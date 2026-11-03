using Aban360.UserPool.Application.Features.SubscriberRequests;
using Aban360.UserPool.Domain.Features.SubscriberRequests;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.SubscriberRequests;

[Route("v1/subscriber-requests")]
public sealed class SubscriberRequestsController(ISubscriberRequestService service) : BaseController
{
    [HttpGet("definitions")]
    public async Task<IActionResult> Definitions(CancellationToken ct) => Ok(await service.Definitions(ct));

    [HttpPost]
    [RequestSizeLimit(128 * 1024)]
    public async Task<IActionResult> Create([FromBody] SubscriberRequestCreateDto input, CancellationToken ct) =>
        Ok(await service.Create(input, CurrentUser.UserId, ct));

    [HttpPost("{id:guid}/confirm")]
    [RequestSizeLimit(1024)]
    public async Task<IActionResult> Confirm(Guid id, [FromBody] SubscriberRequestConfirmDto input, CancellationToken ct) =>
        Ok(await service.Confirm(id, input.Code, CurrentUser.UserId, ct));

    [HttpPost("{id:guid}/resend-code")]
    public async Task<IActionResult> Resend(Guid id, CancellationToken ct) =>
        Ok(await service.Resend(id, CurrentUser.UserId, ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct) => Ok(await service.Get(id, CurrentUser.UserId, ct));
}
