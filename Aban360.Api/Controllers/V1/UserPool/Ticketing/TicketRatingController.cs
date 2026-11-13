using Aban360.UserPool.Application.Features.Ticketing.Services;
using Aban360.Api.Models.Ticketing;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Ticketing.Validations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.UserPool.Ticketing
{
    public class TicketRatingController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITicketRatingService _service;
        public TicketRatingController(IUnitOfWork uow, ITicketRatingService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("~/v1/ticket-rating/create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] TicketRatingCreateDto inputDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(inputDto, CurrentUser, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(true);
        }

        [HttpPost, HttpPatch]
        [Route("~/v1/ticket-rating/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] TicketRatingUpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(inputDto, CurrentUser, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(true);
        }

        [HttpGet]
        [Route("~/v1/ticket-rating/read-by-message/{messageId:long}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketRatingGetDto?>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromTicketRatingGetByMessage([FromRoute] long messageId, CancellationToken cancellationToken)
        {
            var result = await _service.GetByMessageAsync(messageId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/ticket-rating/read/{id:long}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketRatingGetDto>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromTicketRatingGetSingle([FromRoute] long id, CancellationToken cancellationToken)
        {
            var result = await _service.GetSingleAsync(id, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
