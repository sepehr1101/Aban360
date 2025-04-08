using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Persistence.Contexts.Contracts;
using Aban360.WorkflowPool.Application.Features.Design.Handlers.Commands.Update.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.WorkflowPool.Design.Commands
{
    [Route("v1/state")]
    public class StateUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IStateUpdateHandler _stateUpdateHandler;
        public StateUpdateController(
            IUnitOfWork uow,
            IStateUpdateHandler stateUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _stateUpdateHandler = stateUpdateHandler;
            _stateUpdateHandler.NotNull(nameof(stateUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<StateUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] StateUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _stateUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }


}
