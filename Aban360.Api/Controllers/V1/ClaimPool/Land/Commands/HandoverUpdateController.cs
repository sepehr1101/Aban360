using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land.Commands
{
    [Route("v1/handover")]
    public class HandoverUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IHandoverUpdateHandler _handoverUpdateHandler;
        public HandoverUpdateController(
            IUnitOfWork uow,
            IHandoverUpdateHandler handoverUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _handoverUpdateHandler = handoverUpdateHandler;
            _handoverUpdateHandler.NotNull(nameof(handoverUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<HandoverUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] HandoverUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _handoverUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
