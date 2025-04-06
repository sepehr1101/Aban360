using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Queries.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.WorkflowStatusPool.Design.Queries
{
    [Route("v1/workflow_status")]
    public class WorkflowStatusGetAllController : BaseController
    {
        private readonly IWorkflowStatusGetAllHandler _workflowStatusGetAllHandler;
        public WorkflowStatusGetAllController(IWorkflowStatusGetAllHandler workflowStatusGetAllHandler)
        {
            _workflowStatusGetAllHandler = workflowStatusGetAllHandler;
            _workflowStatusGetAllHandler.NotNull(nameof(workflowStatusGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WorkflowStatusGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var workflowStatuss = await _workflowStatusGetAllHandler.Handle(cancellationToken);
            return Ok(workflowStatuss);
        }
    }

}
