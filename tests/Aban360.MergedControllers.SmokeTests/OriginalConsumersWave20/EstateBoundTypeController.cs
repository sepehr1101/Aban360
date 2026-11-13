using Aban360.Api.Controllers.V1;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumersWave20.Aban360.Api.Controllers.V1.ClaimPool.Land
{
    [Route("v1/estate-bound-type")]
    public class EstateBoundTypeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEstateBoundTypeCreateHandler _estateBoundTypeCreateHandler;
        private readonly IEstateBoundTypeUpdateHandler _estateBoundTypeUpdateHandler;
        private readonly IEstateBoundTypeDeleteHandler _estateBoundTypeDeleteHandler;
        private readonly IEstateBoundTypeGetSingleHandler _estateBoundTypeGetSingleHandler;
        private readonly IEstateBoundTypeGetAllHandler _estateBoundTypeGetAllHandler;

        public EstateBoundTypeController(
            IUnitOfWork uow,
            IEstateBoundTypeCreateHandler estateBoundTypeCreateHandler,
            IEstateBoundTypeUpdateHandler estateBoundTypeUpdateHandler,
            IEstateBoundTypeDeleteHandler estateBoundTypeDeleteHandler,
            IEstateBoundTypeGetSingleHandler estateBoundTypeGetSingleHandler,
            IEstateBoundTypeGetAllHandler estateBoundTypeGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(_uow));

            _estateBoundTypeCreateHandler = estateBoundTypeCreateHandler;
            _estateBoundTypeCreateHandler.NotNull(nameof(_estateBoundTypeCreateHandler));

            _estateBoundTypeUpdateHandler = estateBoundTypeUpdateHandler;
            _estateBoundTypeUpdateHandler.NotNull(nameof(_estateBoundTypeUpdateHandler));

            _estateBoundTypeDeleteHandler = estateBoundTypeDeleteHandler;
            _estateBoundTypeDeleteHandler.NotNull(nameof(_estateBoundTypeDeleteHandler));

            _estateBoundTypeGetSingleHandler = estateBoundTypeGetSingleHandler;
            _estateBoundTypeGetSingleHandler.NotNull(nameof(_estateBoundTypeGetSingleHandler));

            _estateBoundTypeGetAllHandler = estateBoundTypeGetAllHandler;
            _estateBoundTypeGetAllHandler.NotNull(nameof(_estateBoundTypeGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateBoundTypeCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] EstateBoundTypeCreateDto createDto, CancellationToken cancellationToken)
        {
            await _estateBoundTypeCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateBoundTypeUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] EstateBoundTypeUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _estateBoundTypeUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }


        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateBoundTypeDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] EstateBoundTypeDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _estateBoundTypeDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpGet, HttpPost]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateBoundTypeGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            EstateBoundTypeGetDto estateBoundType = await _estateBoundTypeGetSingleHandler.Handle(id, cancellationToken);
            return Ok(estateBoundType);
        }

        [HttpGet, HttpPost]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<EstateBoundTypeGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<EstateBoundTypeGetDto> estateBoundType = await _estateBoundTypeGetAllHandler.Handle(cancellationToken);
            return Ok(estateBoundType);
        }
    }
}
