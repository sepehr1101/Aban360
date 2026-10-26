using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Ticketing.Queries
{
    [Route("v1/ticket-status")]
    public class TicketStatusGetAllController : BaseController
    {
        private readonly ITicketStatusGetAllHandler _handler;

        public TicketStatusGetAllController(ITicketStatusGetAllHandler handler)
        {
            _handler = handler;
            _handler.NotNull(nameof(handler));
        }

        [HttpGet]
        [Route("list")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<TicketStatusGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Read(CancellationToken cancellationToken)
        {
            var result = await _handler.Handle(cancellationToken);
            return Ok(result);
        }
    }
}
