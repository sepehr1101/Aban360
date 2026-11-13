using Aban360.WorkflowPool.Application.Features.Design.Services;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
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
        private readonly IWorkflowService _service;
        public WorkflowController(IUnitOfWork uow, IWorkflowService service)
        {
            uow.NotNull(nameof(uow));
            _uow = uow;
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WorkflowCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] WorkflowCreateDto createDto, CancellationToken cancellationToken)
        {
            await _service.CreateAsync(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);
            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] WorkflowUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WorkflowDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] WorkflowDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WorkflowGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var workflows = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(workflows);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WorkflowGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var workflows = await _service.GetAllAsync(cancellationToken);
            return Ok(workflows);
        }

        [HttpPost, HttpPatch]
        [Route("json_definition_update")]//ToDo : Change Route 
        public async Task<IActionResult> Update([FromBody] WorkflowJsonDefinitionUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateDefinitionAsync(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpGet]
        [Route("master")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WorkflowGetMasterDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMaster(CancellationToken cancellationToken)
        {
            ICollection<WorkflowGetMasterDto> workflows = await _service.GetMasterAsync(cancellationToken);
            return Ok(workflows);
        }
    }
}
