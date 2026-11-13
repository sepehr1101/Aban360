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
    public class TicketAttachmentController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITicketAttachmentService _service;
        public TicketAttachmentController(IUnitOfWork uow, ITicketAttachmentService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("~/v1/ticket-attachment/create")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(TicketFileFormMapper.MaxSingleFileRequestBodySize)]
        [RequestFormLimits(MultipartBodyLengthLimit = TicketFileCreateValidator.MaxFileSizeBytes)]
        [ProducesResponseType(typeof(ApiResponseEnvelope<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromForm] TicketAttachmentCreateFormDto inputDto, CancellationToken cancellationToken)
        {
            var file = await TicketFileFormMapper.Read(inputDto.File, cancellationToken);
            await _service.CreateAsync(new TicketAttachmentCreateDto { TicketMessageId = inputDto.TicketMessageId, OriginalFileName = file.OriginalFileName, ContentType = file.ContentType, FileContent = file.FileContent }, CurrentUser, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(true);
        }

        [HttpPost, HttpPatch]
        [Route("~/v1/ticket-attachment/update")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(TicketFileFormMapper.MaxSingleFileRequestBodySize)]
        [RequestFormLimits(MultipartBodyLengthLimit = TicketFileCreateValidator.MaxFileSizeBytes)]
        [ProducesResponseType(typeof(ApiResponseEnvelope<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromForm] TicketAttachmentUpdateFormDto inputDto, CancellationToken cancellationToken)
        {
            var file = await TicketFileFormMapper.Read(inputDto.File, cancellationToken);
            await _service.UpdateAsync(new TicketAttachmentUpdateDto { Id = inputDto.Id, OriginalFileName = file.OriginalFileName, ContentType = file.ContentType, FileContent = file.FileContent }, CurrentUser, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(true);
        }

        [HttpGet]
        [Route("~/v1/ticket-attachment/content/{id:long}")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status206PartialContent)]
        [ProducesResponseType(StatusCodes.Status416RangeNotSatisfiable)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromTicketAttachmentContentGet([FromRoute] long id, CancellationToken cancellationToken, [FromQuery] bool download = false)
        {
            var content = await _service.GetContentAsync(id, CurrentUser, cancellationToken);
            Response.Headers["X-Content-Type-Options"] = "nosniff";
            return new FileContentResult(content.FileContent, content.ContentType)
            {
                EnableRangeProcessing = true,
                FileDownloadName = download ? content.OriginalFileName : string.Empty
            };
        }

        [HttpGet]
        [Route("~/v1/ticket-attachment/list/{messageId:long}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<TicketAttachmentInfo>>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromTicketAttachmentGetList([FromRoute] long messageId, CancellationToken cancellationToken)
        {
            var result = await _service.GetListAsync(messageId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/ticket-attachment/read/{id:long}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketAttachmentInfo>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromTicketAttachmentGetSingle([FromRoute] long id, CancellationToken cancellationToken)
        {
            var result = await _service.GetSingleAsync(id, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
