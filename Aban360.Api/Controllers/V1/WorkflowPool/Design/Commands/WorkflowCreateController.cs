using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Commands.Create.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands;
using Aban360.WorkflowPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.WorkflowPool.Design.Commands
{
    [Route("v1/workflow")]
    public class WorkflowCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWorkflowCreateHandler _workflowCreateHandler;
        public WorkflowCreateController(
            IUnitOfWork uow,
            IWorkflowCreateHandler workflowCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _workflowCreateHandler = workflowCreateHandler;
            _workflowCreateHandler.NotNull(nameof(workflowCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WorkflowCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] WorkflowCreateDto createDto, CancellationToken cancellationToken)
        {
            await _workflowCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            //"Exception occurred: Message: An error occurred while saving the entity changes.
            //See the inner exception for details., Inner Exception:The conversion of a datetime2
            //data type to a datetime data type resulted in an out-of-range value."

            //Error to Create Workflow

            return Ok(createDto);
        }
    }
}
