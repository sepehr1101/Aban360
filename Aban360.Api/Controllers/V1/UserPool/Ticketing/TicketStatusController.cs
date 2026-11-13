using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.UserPool.Ticketing
{
    public class TicketStatusController : BaseController
    {
        private readonly ITicketStatusGetAllHandler _c0_handler;
        private readonly ITicketStatusGetSingleHandler _c1_handler;
        public TicketStatusController(ITicketStatusGetAllHandler c0_handler, ITicketStatusGetSingleHandler c1_handler)
        {
            _c0_handler = c0_handler;
            _c0_handler.NotNull(nameof(c0_handler));
            _c1_handler = c1_handler;
            _c1_handler.NotNull(nameof(c1_handler));
        }

        [HttpGet]
        [Route("~/v1/ticket-status/list")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<TicketStatusGetDto>>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromTicketStatusGetAll(CancellationToken cancellationToken)
        {
            var result = await _c0_handler.Handle(cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/ticket-status/read/{id:int}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketStatusGetDto>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromTicketStatusGetSingle([FromRoute] short id, CancellationToken cancellationToken)
        {
            var result = await _c1_handler.Handle(id, cancellationToken);
            return Ok(result);
        }
    }
}
