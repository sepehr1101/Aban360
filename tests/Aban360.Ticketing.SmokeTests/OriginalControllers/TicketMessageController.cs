using Aban360.Api.Controllers.V1;
using Aban360.Api.Models.Ticketing;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Validations;
using Aban360.UserPool.Application.Features.Ticketing.Services.Contracts;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Queries.Contracts;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.Api.Filters;

namespace BeforeTicketConsumers.Aban360.Api.Controllers.V1.UserPool.Ticketing
{
    public class TicketMessageController : BaseController
    {
        private readonly IUnitOfWork _c0_uow;
        private readonly ITicketMessageCreateHandler _c0_handler;
        private readonly ITicketNotificationService _c0_notifications;
        private readonly IUnitOfWork _c1_uow;
        private readonly ITicketMessageUpdateHandler _c1_handler;
        private readonly ITicketMessageGetListHandler _c2_handler;
        private readonly ITicketMessageGetSingleHandler _c3_handler;
        public TicketMessageController(IUnitOfWork c0_uow, ITicketMessageCreateHandler c0_handler, ITicketNotificationService c0_notifications, IUnitOfWork c1_uow, ITicketMessageUpdateHandler c1_handler, ITicketMessageGetListHandler c2_handler, ITicketMessageGetSingleHandler c3_handler)
        {
            _c0_uow = c0_uow;
            _c0_uow.NotNull(nameof(c0_uow));
            _c0_handler = c0_handler;
            _c0_handler.NotNull(nameof(c0_handler));
            _c0_notifications = c0_notifications;
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
        [Route("~/v1/ticket-message/create")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(TicketFileFormMapper.MaxRequestBodySize)]
        [RequestFormLimits(MultipartBodyLengthLimit = TicketFileCreateValidator.MaxFileSizeBytes)]
        [ProducesResponseType(typeof(ApiResponseEnvelope<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromForm] TicketMessageCreateFormDto inputDto, CancellationToken cancellationToken)
        {
            var createDto = new TicketMessageCreateDto
            {
                TicketId = inputDto.TicketId,
                Body = inputDto.Body,
                Attachments = await TicketFileFormMapper.Read(inputDto.Attachments, cancellationToken)
            };
            await _c0_handler.Handle(createDto, CurrentUser, cancellationToken);
            await _c0_uow.SaveChangesAsync(cancellationToken);
            await _c0_notifications.Answered(createDto.TicketId, CurrentUser.UserId);
            return Ok(true);
        }

        [HttpPost, HttpPatch]
        [Route("~/v1/ticket-message/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] TicketMessageUpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _c1_handler.Handle(inputDto, CurrentUser, cancellationToken);
            await _c1_uow.SaveChangesAsync(cancellationToken);
            return Ok(true);
        }

        [HttpGet]
        [Route("~/v1/ticket-message/list")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketPage<TicketMessageGetDto>>), StatusCodes.Status200OK)]
        public Task<IActionResult> ReadFromQuery([FromQuery] TicketMessageSearchInput inputDto, CancellationToken cancellationToken)
        {
            return ReadFromTicketMessageGetList(inputDto, cancellationToken);
        }

        [HttpPost]
        [Route("~/v1/ticket-message/list")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketPage<TicketMessageGetDto>>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromTicketMessageGetList([FromBody] TicketMessageSearchInput inputDto, CancellationToken cancellationToken)
        {
            var result = await _c2_handler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/ticket-message/read/{id:long}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketMessageGetDto>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromTicketMessageGetSingle([FromRoute] long id, CancellationToken cancellationToken)
        {
            var result = await _c3_handler.Handle(id, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}

