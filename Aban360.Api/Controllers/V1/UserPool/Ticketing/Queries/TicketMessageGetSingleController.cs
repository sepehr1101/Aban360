using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Ticketing.Queries
{
    [Route("v1/ticket-message")]
    public class TicketMessageGetSingleController : BaseController
    {
        private readonly ITicketMessageGetSingleHandler _handler;

        public TicketMessageGetSingleController(ITicketMessageGetSingleHandler handler)
        {
            _handler = handler;
            _handler.NotNull(nameof(handler));
        }

        [HttpGet]
        [Route("read/{id:long}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketMessageGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Read([FromRoute] long id, CancellationToken cancellationToken)
        {
            var result = await _handler.Handle(id, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
