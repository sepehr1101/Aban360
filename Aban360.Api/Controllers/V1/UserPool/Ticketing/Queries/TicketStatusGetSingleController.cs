using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Ticketing.Queries
{
    [Route("v1/ticket-status")]
    public class TicketStatusGetSingleController : BaseController
    {
        private readonly ITicketStatusGetSingleHandler _handler;

        public TicketStatusGetSingleController(ITicketStatusGetSingleHandler handler)
        {
            _handler = handler;
            _handler.NotNull(nameof(handler));
        }

        [HttpGet]
        [Route("read/{id:int}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketStatusGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Read([FromRoute] short id, CancellationToken cancellationToken)
        {
            var result = await _handler.Handle(id, cancellationToken);
            return Ok(result);
        }
    }
}
