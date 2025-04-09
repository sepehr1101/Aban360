using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Commands.Update.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands;
using Aban360.WorkflowPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.WorkflowPool.Design.Commands
{
    [Route("v1/workflow")]
    public class WorkflowJsonDefinitionUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWorkflowJsonDefinitionUpdateHandler _workflowJsonDefinitionUpdateHandler;
        public WorkflowJsonDefinitionUpdateController(
            IUnitOfWork uow,
            IWorkflowJsonDefinitionUpdateHandler workflowJsonDefinitionUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _workflowJsonDefinitionUpdateHandler = workflowJsonDefinitionUpdateHandler;
            _workflowJsonDefinitionUpdateHandler.NotNull(nameof(workflowJsonDefinitionUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("json_definition_update")]//ToDo : Change Route 
        public async Task<IActionResult> Update([FromBody] WorkflowJsonDefinitionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _workflowJsonDefinitionUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }

}
