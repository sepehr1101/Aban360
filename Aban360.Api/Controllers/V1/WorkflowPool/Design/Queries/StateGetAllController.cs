using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Application.Features.Design.Handlers.Queries.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.WorkflowPool.Design.Queries
{
    [Route("v1/state")]
    public class StateGetAllController : BaseController
    {
        private readonly IStateGetAllHandler _stateGetAllHandler;
        public StateGetAllController(IStateGetAllHandler stateGetAllHandler)
        {
            _stateGetAllHandler = stateGetAllHandler;
            _stateGetAllHandler.NotNull(nameof(stateGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<StateGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var states = await _stateGetAllHandler.Handle(cancellationToken);
            return Ok(states);
        }
    }


}
