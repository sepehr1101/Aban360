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

namespace Aban360.Api.Controllers.V1.UserPool.Ticketing
{
    public class TicketController : BaseController
    {
        private readonly IUnitOfWork _c0_uow;
        private readonly ITicketCreateHandler _c0_handler;
        private readonly ITicketNotificationService _c0_notifications;
        private readonly IUnitOfWork _c1_uow;
        private readonly ITicketUpdateHandler _c1_handler;
        private readonly ITicketGetListHandler _c2_handler;
        private readonly ITicketGetMyListHandler _c3_handler;
        private readonly ITicketGetSingleHandler _c4_handler;
        private readonly ITicketReportGetHandler _c5_handler;
        public TicketController(IUnitOfWork c0_uow, ITicketCreateHandler c0_handler, ITicketNotificationService c0_notifications, IUnitOfWork c1_uow, ITicketUpdateHandler c1_handler, ITicketGetListHandler c2_handler, ITicketGetMyListHandler c3_handler, ITicketGetSingleHandler c4_handler, ITicketReportGetHandler c5_handler)
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
            _c4_handler = c4_handler;
            _c4_handler.NotNull(nameof(c4_handler));
            _c5_handler = c5_handler;
            _c5_handler.NotNull(nameof(c5_handler));
        }

        [HttpPost]
        [Route("~/v1/ticket/create")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(TicketFileFormMapper.MaxRequestBodySize)]
        [RequestFormLimits(MultipartBodyLengthLimit = TicketFileCreateValidator.MaxFileSizeBytes)]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketCreateResultDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromForm] TicketCreateFormDto inputDto, CancellationToken cancellationToken)
        {
            var createDto = new TicketCreateDto
            {
                Subject = inputDto.Subject,
                Body = inputDto.Body,
                Attachments = await TicketFileFormMapper.Read(inputDto.Attachments, cancellationToken)
            };
            var ticket = await _c0_handler.Handle(createDto, CurrentUser, cancellationToken);
            await _c0_uow.SaveChangesAsync(cancellationToken);
            var result = new TicketCreateResultDto
            {
                Id = ticket.Id,
                TicketNumber = ticket.TicketNumber
            };
            await _c0_notifications.Created(ticket.TicketNumber);
            return Ok(result);
        }

        [HttpPost, HttpPatch]
        [Route("~/v1/ticket/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] TicketUpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _c1_handler.Handle(inputDto, CurrentUser, cancellationToken);
            await _c1_uow.SaveChangesAsync(cancellationToken);
            return Ok(true);
        }

        [HttpGet]
        [Route("~/v1/ticket/list")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketPage<TicketGetDto>>), StatusCodes.Status200OK)]
        [ActionName("ReadFromQuery")]
        public Task<IActionResult> ReadFromQueryFromTicketGetList([FromQuery] TicketSearchInput inputDto, CancellationToken cancellationToken)
        {
            return ReadFromTicketGetList(inputDto, cancellationToken);
        }

        [HttpPost]
        [Route("~/v1/ticket/list")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketPage<TicketGetDto>>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromTicketGetList([FromBody] TicketSearchInput inputDto, CancellationToken cancellationToken)
        {
            var result = await _c2_handler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/ticket/my-list")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketPage<TicketGetDto>>), StatusCodes.Status200OK)]
        [ActionName("ReadFromQuery")]
        public Task<IActionResult> ReadFromQueryFromTicketGetMyList([FromQuery] TicketMySearchInput inputDto, CancellationToken cancellationToken)
        {
            return ReadFromTicketGetMyList(inputDto, cancellationToken);
        }

        [HttpPost]
        [Route("~/v1/ticket/my-list")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketPage<TicketGetDto>>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromTicketGetMyList([FromBody] TicketMySearchInput inputDto, CancellationToken cancellationToken)
        {
            var result = await _c3_handler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/ticket/read/{id:long}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketGetDto>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromTicketGetSingle([FromRoute] long id, CancellationToken cancellationToken)
        {
            var result = await _c4_handler.Handle(id, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/ticket/report")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketReportOutput>), StatusCodes.Status200OK)]
        public Task<IActionResult> ReportFromQuery([FromQuery] TicketSearchInput inputDto, CancellationToken cancellationToken)
        {
            return Report(inputDto, cancellationToken);
        }

        [HttpPost]
        [Route("~/v1/ticket/report")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketReportOutput>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Report([FromBody] TicketSearchInput inputDto, CancellationToken cancellationToken)
        {
            var result = await _c5_handler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/ticket/my-report")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketReportOutput>), StatusCodes.Status200OK)]
        public Task<IActionResult> MyReportFromQuery([FromQuery] TicketMySearchInput inputDto, CancellationToken cancellationToken)
        {
            return MyReport(inputDto, cancellationToken);
        }

        [HttpPost]
        [Route("~/v1/ticket/my-report")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketReportOutput>), StatusCodes.Status200OK)]
        public async Task<IActionResult> MyReport([FromBody] TicketMySearchInput inputDto, CancellationToken cancellationToken)
        {
            var result = await _c5_handler.HandleMine(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
