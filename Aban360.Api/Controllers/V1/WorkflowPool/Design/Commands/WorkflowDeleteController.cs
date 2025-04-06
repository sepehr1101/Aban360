using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Commands.Delete.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands;
using Aban360.WorkflowPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.WorkflowPool.Design.Commands
{
    [Route("v1/workflow")]
    public class WorkflowDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWorkflowDeleteHandler _workflowDeleteHandler;
        public WorkflowDeleteController(
            IUnitOfWork uow,
            IWorkflowDeleteHandler workflowDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _workflowDeleteHandler = workflowDeleteHandler;
            _workflowDeleteHandler.NotNull(nameof(workflowDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WorkflowDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] WorkflowDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _workflowDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }

}
