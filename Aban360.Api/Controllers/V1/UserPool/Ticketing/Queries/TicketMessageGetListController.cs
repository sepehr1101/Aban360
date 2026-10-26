using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Ticketing.Queries
{
    [Route("v1/ticket-message")]
    public class TicketMessageGetListController : BaseController
    {
        private readonly ITicketMessageGetListHandler _handler;

        public TicketMessageGetListController(ITicketMessageGetListHandler handler)
        {
            _handler = handler;
            _handler.NotNull(nameof(handler));
        }

        [HttpPost]
        [Route("list")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketPage<TicketMessageGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Read([FromBody] TicketMessageSearchInput inputDto, CancellationToken cancellationToken)
        {
            var result = await _handler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
