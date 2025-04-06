using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Commands.Update.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands;
using Aban360.WorkflowPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.WorkflowPool.Design.Commands
{
    [Route("v1/workflow")]
    public class WorkflowUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWorkflowUpdateHandler _workflowUpdateHandler;
        public WorkflowUpdateController(
            IUnitOfWork uow,
            IWorkflowUpdateHandler workflowUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _workflowUpdateHandler = workflowUpdateHandler;
            _workflowUpdateHandler.NotNull(nameof(workflowUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] WorkflowUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _workflowUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }

}
