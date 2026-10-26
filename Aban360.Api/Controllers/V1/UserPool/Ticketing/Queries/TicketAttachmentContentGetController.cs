using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Queries.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Ticketing.Queries
{
    [Route("v1/ticket-attachment")]
    public class TicketAttachmentContentGetController : BaseController
    {
        private readonly ITicketAttachmentContentGetHandler _handler;

        public TicketAttachmentContentGetController(ITicketAttachmentContentGetHandler handler)
        {
            _handler = handler;
            _handler.NotNull(nameof(handler));
        }

        [HttpGet]
        [Route("content/{id:long}")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status206PartialContent)]
        [ProducesResponseType(StatusCodes.Status416RangeNotSatisfiable)]
        public async Task<IActionResult> Read([FromRoute] long id, CancellationToken cancellationToken,
            [FromQuery] bool download = false)
        {
            var content = await _handler.Handle(id, CurrentUser, cancellationToken);
            Response.Headers["X-Content-Type-Options"] = "nosniff";
            return new FileContentResult(content.FileContent, content.ContentType)
            {
                EnableRangeProcessing = true,
                FileDownloadName = download ? content.OriginalFileName : string.Empty
            };
        }
    }
}
