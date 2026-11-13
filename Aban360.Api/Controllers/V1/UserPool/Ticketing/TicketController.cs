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
    public class TicketController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITicketService _service;
        private readonly ITicketNotificationService _notifications;
        public TicketController(IUnitOfWork uow, ITicketService service, ITicketNotificationService notifications)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
            notifications.NotNull(nameof(notifications));
            _notifications = notifications;
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
            var ticket = await _service.CreateAsync(createDto, CurrentUser, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            var result = new TicketCreateResultDto
            {
                Id = ticket.Id,
                TicketNumber = ticket.TicketNumber
            };
            await _notifications.Created(ticket.TicketNumber);
            return Ok(result);
        }

        [HttpPost, HttpPatch]
        [Route("~/v1/ticket/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] TicketUpdateDto inputDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(inputDto, CurrentUser, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
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
            var result = await _service.GetListAsync(inputDto, CurrentUser, cancellationToken);
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
            var result = await _service.GetMyListAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/ticket/read/{id:long}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketGetDto>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromTicketGetSingle([FromRoute] long id, CancellationToken cancellationToken)
        {
            var result = await _service.GetSingleAsync(id, CurrentUser, cancellationToken);
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
            var result = await _service.GetReportAsync(inputDto, CurrentUser, cancellationToken);
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
            var result = await _service.GetMyReportAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
