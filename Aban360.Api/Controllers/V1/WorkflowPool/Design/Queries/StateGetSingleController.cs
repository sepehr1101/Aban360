using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Application.Features.Design.Handlers.Queries.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.WorkflowPool.Design.Queries
{
    [Route("v1/state")]
    public class StateGetSingleController : BaseController
    {
        private readonly IStateGetSingleHandler _stateGetSingleHandler;
        public StateGetSingleController(IStateGetSingleHandler stateGetSingleHandler)
        {
            _stateGetSingleHandler = stateGetSingleHandler;
            _stateGetSingleHandler.NotNull(nameof(stateGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<StateGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            var states = await _stateGetSingleHandler.Handle(id, cancellationToken);
            return Ok(states);
        }
    }


}
