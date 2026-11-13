using Aban360.UserPool.Application.Features.Ticketing.Services;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.UserPool.Ticketing
{
    public class TicketStatusController : BaseController
    {
        private readonly ITicketStatusService _service;

        public TicketStatusController(ITicketStatusService service)
        {
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpGet]
        [Route("~/v1/ticket-status/list")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<TicketStatusGetDto>>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromTicketStatusGetAll(CancellationToken cancellationToken)
        {
            var result = await _service.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/ticket-status/read/{id:int}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketStatusGetDto>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromTicketStatusGetSingle([FromRoute] short id, CancellationToken cancellationToken)
        {
            var result = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(result);
        }
    }
}
