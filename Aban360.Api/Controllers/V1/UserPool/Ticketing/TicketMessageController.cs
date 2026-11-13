using Aban360.UserPool.Application.Features.Ticketing.Services;
using Aban360.Api.Models.Ticketing;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Ticketing.Validations;
using Aban360.UserPool.Application.Features.Ticketing.Services.Contracts;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Queries;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.UserPool.Ticketing
{
    public class TicketMessageController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITicketMessageService _service;
        private readonly ITicketNotificationService _notifications;
        public TicketMessageController(IUnitOfWork uow, ITicketMessageService service, ITicketNotificationService notifications)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
            notifications.NotNull(nameof(notifications));
            _notifications = notifications;
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
            await _service.CreateAsync(createDto, CurrentUser, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            await _notifications.Answered(createDto.TicketId, CurrentUser.UserId);
            return Ok(true);
        }

        [HttpPost, HttpPatch]
        [Route("~/v1/ticket-message/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] TicketMessageUpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(inputDto, CurrentUser, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
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
            var result = await _service.GetListAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/ticket-message/read/{id:long}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketMessageGetDto>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromTicketMessageGetSingle([FromRoute] long id, CancellationToken cancellationToken)
        {
            var result = await _service.GetSingleAsync(id, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
