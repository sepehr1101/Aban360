using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.WorkflowPool.Persistence.Contexts.Contracts;
using Aban360.WorkflowPool.Application.Features.Design.Handlers.Commands.Delete.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Commands;
using Microsoft.AspNetCore.Mvc;
using Aban360.WorkflowPool.Application.Features.Design.Handlers.Commands.Update.Contracts;
using Aban360.WorkflowPool.Application.Features.Design.Handlers.Queries.Contracts;
using Aban360.WorkflowPool.Domain.Features.Design.Dto.Queries;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.WorkflowPool.Design
{
    public class StateController : BaseController
    {
        private readonly IUnitOfWork _c0_uow;
        private readonly IStateDeleteHandler _c0_stateDeleteHandler;
        private readonly IUnitOfWork _c1_uow;
        private readonly IStateUpdateHandler _c1_stateUpdateHandler;
        private readonly IStateGetAllHandler _c2_stateGetAllHandler;
        private readonly IStateGetSingleHandler _c3_stateGetSingleHandler;
        public StateController(IUnitOfWork c0_uow, IStateDeleteHandler c0_stateDeleteHandler, IUnitOfWork c1_uow, IStateUpdateHandler c1_stateUpdateHandler, IStateGetAllHandler c2_stateGetAllHandler, IStateGetSingleHandler c3_stateGetSingleHandler)
        {
            _c0_uow = c0_uow;
            _c0_uow.NotNull(nameof(c0_uow));
            _c0_stateDeleteHandler = c0_stateDeleteHandler;
            _c0_stateDeleteHandler.NotNull(nameof(c0_stateDeleteHandler));
            _c1_uow = c1_uow;
            _c1_uow.NotNull(nameof(c1_uow));
            _c1_stateUpdateHandler = c1_stateUpdateHandler;
            _c1_stateUpdateHandler.NotNull(nameof(c1_stateUpdateHandler));
            _c2_stateGetAllHandler = c2_stateGetAllHandler;
            _c2_stateGetAllHandler.NotNull(nameof(c2_stateGetAllHandler));
            _c3_stateGetSingleHandler = c3_stateGetSingleHandler;
            _c3_stateGetSingleHandler.NotNull(nameof(c3_stateGetSingleHandler));
        }

        [HttpPost, HttpDelete]
        [Route("~/v1/state/delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<StateDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] StateDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _c0_stateDeleteHandler.Handle(deleteDto, cancellationToken);
            await _c0_uow.SaveChangesAsync(cancellationToken);
            return Ok(deleteDto);
        }

        [HttpPost, HttpPatch]
        [Route("~/v1/state/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<StateUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] StateUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _c1_stateUpdateHandler.Handle(updateDto, cancellationToken);
            await _c1_uow.SaveChangesAsync(cancellationToken);
            return Ok(updateDto);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/state/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<StateGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var states = await _c2_stateGetAllHandler.Handle(cancellationToken);
            return Ok(states);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/state/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<StateGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            var states = await _c3_stateGetSingleHandler.Handle(id, cancellationToken);
            return Ok(states);
        }
    }
}
