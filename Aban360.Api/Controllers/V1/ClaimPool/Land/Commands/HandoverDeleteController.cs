using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/handover")]
    public class HandoverDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IHandoverDeleteHandler _handoverDeleteHandler;
        public HandoverDeleteController(
            IUnitOfWork uow,
            IHandoverDeleteHandler handoverDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _handoverDeleteHandler = handoverDeleteHandler;
            _handoverDeleteHandler.NotNull(nameof(handoverDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<HandoverDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] HandoverDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _handoverDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
