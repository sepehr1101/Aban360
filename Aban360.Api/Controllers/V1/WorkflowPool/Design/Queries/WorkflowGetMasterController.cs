using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Queries.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.WorkflowPool.Design.Queries
{
    [Route("v1/workflow")]
    public class WorkflowGetMasterController : BaseController
    {
        private readonly IWorkflowGetMasterHandler _workflowGetMasterHandler;
        public WorkflowGetMasterController(IWorkflowGetMasterHandler workflowGetMasterHandler)
        {
            _workflowGetMasterHandler = workflowGetMasterHandler;
            _workflowGetMasterHandler.NotNull(nameof(workflowGetMasterHandler));
        }

        [HttpPost, HttpGet]
        [Route("master")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WorkflowGetMasterDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMaster(CancellationToken cancellationToken)
        {
            ICollection<WorkflowGetMasterDto> workflows = await _workflowGetMasterHandler.Handle(cancellationToken);
            return Ok(workflows);
        }
    }

}
