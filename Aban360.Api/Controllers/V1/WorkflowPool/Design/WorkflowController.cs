using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Commands.Create.Contracts;
using Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Commands.Delete.Contracts;
using Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Commands.Update.Contracts;
using Aban360.WorkflowPool.Application.Features.Assignment.Handlers.Queries.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;
using Aban360.WorkflowPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.WorkflowPool.Design
{
    [Route("v1/workflow")]
    public class WorkflowController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWorkflowCreateHandler _workflowCreateHandler;
        private readonly IWorkflowUpdateHandler _workflowUpdateHandler;
        private readonly IWorkflowDeleteHandler _workflowDeleteHandler;
        private readonly IWorkflowGetSingleHandler _workflowGetSingleHandler;
        private readonly IWorkflowGetAllHandler _workflowGetAllHandler;
        private readonly IWorkflowJsonDefinitionUpdateHandler _workflowJsonDefinitionUpdateHandler;
        private readonly IWorkflowGetMasterHandler _workflowGetMasterHandler;

        public WorkflowController(
            IUnitOfWork uow,
            IWorkflowCreateHandler workflowCreateHandler,
            IWorkflowUpdateHandler workflowUpdateHandler,
            IWorkflowDeleteHandler workflowDeleteHandler,
            IWorkflowGetSingleHandler workflowGetSingleHandler,
            IWorkflowGetAllHandler workflowGetAllHandler,
            IWorkflowJsonDefinitionUpdateHandler workflowJsonDefinitionUpdateHandler,
            IWorkflowGetMasterHandler workflowGetMasterHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _workflowCreateHandler = workflowCreateHandler;
            _workflowCreateHandler.NotNull(nameof(workflowCreateHandler));

            _workflowUpdateHandler = workflowUpdateHandler;
            _workflowUpdateHandler.NotNull(nameof(workflowUpdateHandler));

            _workflowDeleteHandler = workflowDeleteHandler;
            _workflowDeleteHandler.NotNull(nameof(workflowDeleteHandler));

            _workflowGetSingleHandler = workflowGetSingleHandler;
            _workflowGetSingleHandler.NotNull(nameof(workflowGetSingleHandler));

            _workflowGetAllHandler = workflowGetAllHandler;
            _workflowGetAllHandler.NotNull(nameof(workflowGetAllHandler));

            _workflowJsonDefinitionUpdateHandler = workflowJsonDefinitionUpdateHandler;
            _workflowJsonDefinitionUpdateHandler.NotNull(nameof(workflowJsonDefinitionUpdateHandler));

            _workflowGetMasterHandler = workflowGetMasterHandler;
            _workflowGetMasterHandler.NotNull(nameof(workflowGetMasterHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WorkflowCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] WorkflowCreateDto createDto, CancellationToken cancellationToken)
        {
            await _workflowCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] WorkflowUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _workflowUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
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

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WorkflowGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var workflows = await _workflowGetSingleHandler.Handle(id, cancellationToken);
            return Ok(workflows);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WorkflowGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var workflows = await _workflowGetAllHandler.Handle(cancellationToken);
            return Ok(workflows);
        }

        [HttpPost, HttpPatch]
        [Route("json_definition_update")]//ToDo : Change Route 
        public async Task<IActionResult> Update([FromBody] WorkflowJsonDefinitionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _workflowJsonDefinitionUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
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
