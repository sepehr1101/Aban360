using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Persistence.Contexts.Contracts;
using Aban360.WorkflowPool.Application.Features.Design.Handlers.Commands.Delete.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.WorkflowPool.Design.Commands
{
    [Route("v1/state")]
    public class StateDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IStateDeleteHandler _stateDeleteHandler;
        public StateDeleteController(
            IUnitOfWork uow,
            IStateDeleteHandler stateDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _stateDeleteHandler = stateDeleteHandler;
            _stateDeleteHandler.NotNull(nameof(stateDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<StateDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] StateDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _stateDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
