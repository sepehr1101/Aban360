using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.Land.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Commands;
using Aban360.ClaimPool.Domain.Features.Land.Dto.Queries;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Land
{
    [Route("v1/estate-water-resource")]
    public class EstateWaterResourceController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEstateWaterResourceCreateHandler _estateWaterResourceCreateHandler;
        private readonly IEstateWaterResourceUpdateHandler _estateWaterResourceUpdateHandler;
        private readonly IEstateWaterResourceDeleteHandler _estateWaterResourceDeleteHandler;
        private readonly IEstateWaterResourceGetSingleHandler _estateWaterResourceGetSingleHandler;
        private readonly IEstateWaterResourceGetAllHandler _estateWaterResourceGetAllHandler;

        public EstateWaterResourceController(
            IUnitOfWork uow,
            IEstateWaterResourceCreateHandler estateWaterResourceCreateHandler,
            IEstateWaterResourceUpdateHandler estateWaterResourceUpdateHandler,
            IEstateWaterResourceDeleteHandler estateWaterResourceDeleteHandler,
            IEstateWaterResourceGetSingleHandler estateWaterResourceGetSingleHandler,
            IEstateWaterResourceGetAllHandler estateWaterResourceGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _estateWaterResourceCreateHandler = estateWaterResourceCreateHandler;
            _estateWaterResourceCreateHandler.NotNull(nameof(estateWaterResourceCreateHandler));

            _estateWaterResourceUpdateHandler = estateWaterResourceUpdateHandler;
            _estateWaterResourceUpdateHandler.NotNull(nameof(estateWaterResourceUpdateHandler));

            _estateWaterResourceDeleteHandler = estateWaterResourceDeleteHandler;
            _estateWaterResourceDeleteHandler.NotNull(nameof(estateWaterResourceDeleteHandler));

            _estateWaterResourceGetSingleHandler = estateWaterResourceGetSingleHandler;
            _estateWaterResourceGetSingleHandler.NotNull(nameof(estateWaterResourceGetSingleHandler));

            _estateWaterResourceGetAllHandler = estateWaterResourceGetAllHandler;
            _estateWaterResourceGetAllHandler.NotNull(nameof(estateWaterResourceGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateWaterResourceCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] EstateWaterResourceCreateDto createDto, CancellationToken cancellationToken)
        {
            await _estateWaterResourceCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateWaterResourceUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] EstateWaterResourceUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _estateWaterResourceUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateWaterResourceDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] EstateWaterResourceDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _estateWaterResourceDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EstateWaterResourceGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            EstateWaterResourceGetDto estateWaterResources = await _estateWaterResourceGetSingleHandler.Handle(id, cancellationToken);
            return Ok(estateWaterResources);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<EstateWaterResourceGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<EstateWaterResourceGetDto> estateWaterResources = await _estateWaterResourceGetAllHandler.Handle(cancellationToken);
            return Ok(estateWaterResources);
        }
    }
}
