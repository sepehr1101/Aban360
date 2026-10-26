using Aban360.Api.Models.Ticketing;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Update.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Validations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Ticketing.Commands
{
    [Route("v1/ticket-attachment")]
    public class TicketAttachmentUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITicketAttachmentUpdateHandler _handler;

        public TicketAttachmentUpdateController(IUnitOfWork uow, ITicketAttachmentUpdateHandler handler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));
            _handler = handler;
            _handler.NotNull(nameof(handler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(TicketFileFormMapper.MaxSingleFileRequestBodySize)]
        [RequestFormLimits(MultipartBodyLengthLimit = TicketFileCreateValidator.MaxFileSizeBytes)]
        [ProducesResponseType(typeof(ApiResponseEnvelope<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromForm] TicketAttachmentUpdateFormDto inputDto, CancellationToken cancellationToken)
        {
            var file = await TicketFileFormMapper.Read(inputDto.File, cancellationToken);
            await _handler.Handle(new TicketAttachmentUpdateDto
            {
                Id = inputDto.Id, OriginalFileName = file.OriginalFileName,
                ContentType = file.ContentType, FileContent = file.FileContent
            }, CurrentUser, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(true);
        }
    }
}
