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

namespace Aban360.Api.Controllers.V1.UserPool.Ticketing
{
    public class TicketAttachmentController : BaseController
    {
        private readonly IUnitOfWork _c0_uow;
        private readonly ITicketAttachmentCreateHandler _c0_handler;
        private readonly IUnitOfWork _c1_uow;
        private readonly ITicketAttachmentUpdateHandler _c1_handler;
        private readonly ITicketAttachmentContentGetHandler _c2_handler;
        private readonly ITicketAttachmentGetListHandler _c3_handler;
        private readonly ITicketAttachmentGetSingleHandler _c4_handler;
        public TicketAttachmentController(IUnitOfWork c0_uow, ITicketAttachmentCreateHandler c0_handler, IUnitOfWork c1_uow, ITicketAttachmentUpdateHandler c1_handler, ITicketAttachmentContentGetHandler c2_handler, ITicketAttachmentGetListHandler c3_handler, ITicketAttachmentGetSingleHandler c4_handler)
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
            _c4_handler = c4_handler;
            _c4_handler.NotNull(nameof(c4_handler));
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
            await _c0_handler.Handle(new TicketAttachmentCreateDto { TicketMessageId = inputDto.TicketMessageId, OriginalFileName = file.OriginalFileName, ContentType = file.ContentType, FileContent = file.FileContent }, CurrentUser, cancellationToken);
            await _c0_uow.SaveChangesAsync(cancellationToken);
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
            await _c1_handler.Handle(new TicketAttachmentUpdateDto { Id = inputDto.Id, OriginalFileName = file.OriginalFileName, ContentType = file.ContentType, FileContent = file.FileContent }, CurrentUser, cancellationToken);
            await _c1_uow.SaveChangesAsync(cancellationToken);
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
            var content = await _c2_handler.Handle(id, CurrentUser, cancellationToken);
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
            var result = await _c3_handler.Handle(messageId, CurrentUser, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [Route("~/v1/ticket-attachment/read/{id:long}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<TicketAttachmentInfo>), StatusCodes.Status200OK)]
        [ActionName("Read")]
        public async Task<IActionResult> ReadFromTicketAttachmentGetSingle([FromRoute] long id, CancellationToken cancellationToken)
        {
            var result = await _c4_handler.Handle(id, CurrentUser, cancellationToken);
            return Ok(result);
        }
    }
}
