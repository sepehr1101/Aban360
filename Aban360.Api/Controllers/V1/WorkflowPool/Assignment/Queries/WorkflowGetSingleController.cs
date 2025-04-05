using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Queries.Contracts;
using Aban360.WorkflowPool.Domain.Features.Assignment.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.WorkflowPool.Assignment.Queries
{
    [Route("v1/workflow")]
    public class WorkflowGetSingleController : BaseController
    {
        private readonly IWorkflowGetSingleHandler _workflowGetSingleHandler;
        public WorkflowGetSingleController(IWorkflowGetSingleHandler workflowGetSingleHandler)
        {
            _workflowGetSingleHandler = workflowGetSingleHandler;
            _workflowGetSingleHandler.NotNull(nameof(workflowGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WorkflowGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var workflows = await _workflowGetSingleHandler.Handle(id, cancellationToken);
            return Ok(workflows);
        }
    }

}
