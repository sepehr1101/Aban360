using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Ticketing.Queries
{
    [Route("v1/ticket-attachment")]
    public class TicketAttachmentGetListController : BaseController
    {
        private readonly ITicketAttachmentGetListHandler _handler;

        public TicketAttachmentGetListController(ITicketAttachmentGetListHandler handler)
        {
            _handler = handler;
            _handler.NotNull(nameof(handler));
        }

        [HttpGet]
        [Route("list/{messageId:long}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<TicketAttachmentInfo>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Read([FromRoute] long messageId, CancellationToken cancellationToken)
        {
            var result = await _handler.Handle(messageId, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
