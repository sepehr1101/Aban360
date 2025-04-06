using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Queries.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.WorkflowPool.Design.Queries
{
    [Route("v1/workflow")]
    public class WorkflowGetAllController : BaseController
    {
        private readonly IWorkflowGetAllHandler _workflowGetAllHandler;
        public WorkflowGetAllController(IWorkflowGetAllHandler workflowGetAllHandler)
        {
            _workflowGetAllHandler = workflowGetAllHandler;
            _workflowGetAllHandler.NotNull(nameof(workflowGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WorkflowGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var workflows = await _workflowGetAllHandler.Handle(cancellationToken);
            return Ok(workflows);
        }
    }

}
