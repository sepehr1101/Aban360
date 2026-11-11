using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Constants;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering
{
    [Route("v1/use-state")]
    public class UseStateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUseStateCreateHandler _useStateCreateHandler;
        private readonly IUseStateUpdateHandler _useStateUpdateHandler;
        private readonly IUseStateDeleteHandler _useStateDeleteHandler;
        private readonly IUseStateGetSingleHandler _useStateGetSingleHandler;
        private readonly IUseStateGetAllHandler _useStateGetAllHandler;

        public UseStateController(
            IUnitOfWork uow,
            IUseStateCreateHandler useStateCreateHandler,
            IUseStateUpdateHandler useStateUpdateHandler,
            IUseStateDeleteHandler useStateDeleteHandler,
            IUseStateGetSingleHandler useStateGetSingleHandler,
            IUseStateGetAllHandler useStateGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _useStateCreateHandler = useStateCreateHandler;
            _useStateCreateHandler.NotNull(nameof(useStateCreateHandler));

            _useStateUpdateHandler = useStateUpdateHandler;
            _useStateUpdateHandler.NotNull(nameof(useStateUpdateHandler));

            _useStateDeleteHandler = useStateDeleteHandler;
            _useStateDeleteHandler.NotNull(nameof(useStateDeleteHandler));

            _useStateGetSingleHandler = useStateGetSingleHandler;
            _useStateGetSingleHandler.NotNull(nameof(useStateGetSingleHandler));

            _useStateGetAllHandler = useStateGetAllHandler;
            _useStateGetAllHandler.NotNull(nameof(useStateGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UseStateCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] UseStateCreateDto createDto, CancellationToken cancellationToken)
        {
            await _useStateCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UseStateUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] UseStateUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _useStateUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UseStateDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] UseStateDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _useStateDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<UseStateGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(UseStateEnum id, CancellationToken cancellationToken)
        {
            UseStateGetDto useEstate = await _useStateGetSingleHandler.Handle(id, cancellationToken);
            return Ok(useEstate);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<UseStateGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<UseStateGetDto> useEstates = await _useStateGetAllHandler.Handle(cancellationToken);
            return Ok(useEstates);
        }
    }
}
