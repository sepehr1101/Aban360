using Aban360.Api.Models.Ticketing;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.UserPool.Application.Features.Ticketing.Handlers.Commands.Create.Contracts;
using Aban360.UserPool.Application.Features.Ticketing.Validations;
using Aban360.UserPool.Domain.Features.Ticketing.Dto.Commands;
using Aban360.UserPool.Persistence.Contexts.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.UserPool.Ticketing.Commands
{
    [Route("v1/ticket-message")]
    public class TicketMessageCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ITicketMessageCreateHandler _handler;

        public TicketMessageCreateController(IUnitOfWork uow, ITicketMessageCreateHandler handler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));
            _handler = handler;
            _handler.NotNull(nameof(handler));
        }

        [HttpPost]
        [Route("create")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(TicketFileFormMapper.MaxRequestBodySize)]
        [RequestFormLimits(MultipartBodyLengthLimit = TicketFileCreateValidator.MaxFileSizeBytes)]
        [ProducesResponseType(typeof(ApiResponseEnvelope<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromForm] TicketMessageCreateFormDto inputDto, CancellationToken cancellationToken)
        {
            var createDto = new TicketMessageCreateDto
            {
                TicketId = inputDto.TicketId, Body = inputDto.Body,
                Attachments = await TicketFileFormMapper.Read(inputDto.Attachments, cancellationToken)
            };
            await _handler.Handle(createDto, CurrentUser, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(true);
        }
    }
}
