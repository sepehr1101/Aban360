using Aban360.Api.Controllers.V1;
using Aban360.Api.Models.Ticketing;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Validations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.Api.Filters;

namespace BeforeTicketConsumers.Aban360.Api.Controllers.V1.UserPool.Ticketing
{
    public class TicketRatingController : BaseController
    {
        private readonly IUnitOfWork _c0_uow;
        private readonly ITicketRatingCreateHandler _c0_handler;
        private readonly IUnitOfWork _c1_uow;
        private readonly ITicketRatingUpdateHandler _c1_handler;
        private readonly ITicketRatingGetByMessageHandler _c2_handler;
        private readonly ITicketRatingGetSingleHandler _c3_handler;
        public TicketRatingController(IUnitOfWork c0_uow, ITicketRatingCreateHandler c0_handler, IUnitOfWork c1_uow, ITicketRatingUpdateHandler c1_handler, ITicketRatingGetByMessageHandler c2_handler, ITicketRatingGetSingleHandler c3_handler)
        {
            _c0_uow = c0_uow;
            _c0_uow.NotNull(nameof(c0_uow));
            _c0_handler = c0_handler;
            _c0_handler.NotNull(nameof(c0_handler));
            _c1_uow = c1_uow;
            _c1_uow.NotNull(nameof(c1_uow));
            _c1_handler = c1_handler;
            _c1_handler.NotNull(nameof(c1_handler));
            _c2_handler = c2_handler;
            _c2_handler.NotNull(nameof(c2_handler));
            _c3_handler = c3_handler;
            _c3_handler.NotNull(nameof(c3_handler));
        }

        [HttpPost]
        [Route("~/v1/ticket-rating/create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] TicketRatingCreateDto inputDto, CancellationToken cancellationToken)
        {
            await _c0_handler.Handle(inputDto, CurrentUser, cancellationToken);
            await _c0_uow.SaveChangesAsync(cancellationToken);
            return Ok(true);
        }

        [HttpPost, HttpPatch]
        [Route("~/v1/ticket-rating/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] TicketRatingUpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _c1_handler.Handle(inputDto, CurrentUser, cancellationToken);
            await _c1_uow.SaveChangesAsync(cancellationToken);
            return Ok(true);
        }

        [HttpGet]
        [Route("~/v1/ticket-rating/read-by-message/{messageId:long}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketRatingGetDto?>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromTicketRatingGetByMessage([FromRoute] long messageId, CancellationToken cancellationToken)
        {
            var result = await _c2_handler.Handle(messageId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/ticket-rating/read/{id:long}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketRatingGetDto>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromTicketRatingGetSingle([FromRoute] long id, CancellationToken cancellationToken)
        {
            var result = await _c3_handler.Handle(id, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}

