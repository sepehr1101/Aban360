using Aban360.WorkflowPool.Application.Features.Design.Services;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Persistence.Contexts.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands;
using Microsoft.AspNetCore.Mvc;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.WorkflowPool.Design
{
    public class StateController : BaseController
    {
        private readonly IUnitOfWork _c0_uow;
        private readonly IStateService _service;
        private readonly IUnitOfWork _c1_uow;

        public StateController(IUnitOfWork c0_uow, IStateService service, IUnitOfWork c1_uow)
        {
            c0_uow.NotNull(nameof(c0_uow));
            _c0_uow = c0_uow;
            service.NotNull(nameof(service));
            _service = service;
            c1_uow.NotNull(nameof(c1_uow));
            _c1_uow = c1_uow;
        }

        [HttpPost, HttpDelete]
        [Route("~/v1/state/delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<StateDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] StateDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(deleteDto, cancellationToken);
            await _c0_uow.SaveChangesAsync(cancellationToken);
            return Ok(deleteDto);
        }

        [HttpPost, HttpPatch]
        [Route("~/v1/state/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<StateUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] StateUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _service.UpdateAsync(updateDto, cancellationToken);
            await _c1_uow.SaveChangesAsync(cancellationToken);
            return Ok(updateDto);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/state/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<StateGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var states = await _service.GetAllAsync(cancellationToken);
            return Ok(states);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/state/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<StateGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            var states = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(states);
        }
    }
}
