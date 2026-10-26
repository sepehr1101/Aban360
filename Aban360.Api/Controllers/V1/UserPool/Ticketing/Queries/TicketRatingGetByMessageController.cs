using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Ticketing.Queries
{
    [Route("v1/ticket-rating")]
    public class TicketRatingGetByMessageController : BaseController
    {
        private readonly ITicketRatingGetByMessageHandler _handler;

        public TicketRatingGetByMessageController(ITicketRatingGetByMessageHandler handler)
        {
            _handler = handler;
            _handler.NotNull(nameof(handler));
        }

        [HttpGet]
        [Route("read-by-message/{messageId:long}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketRatingGetDto?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Read([FromRoute] long messageId, CancellationToken cancellationToken)
        {
            var result = await _handler.Handle(messageId, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
