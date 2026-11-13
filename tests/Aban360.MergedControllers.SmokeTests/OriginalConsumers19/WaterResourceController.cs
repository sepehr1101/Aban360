using Aban360.Api.Controllers.V1;
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

namespace BeforeServiceConsumers19.Aban360.Api.Controllers.V1.ClaimPool.Land
{
    [Route("v1/water-resource")]
    public class WaterResourceController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterResourceCreateHandler _waterResourceCreateHandler;
        private readonly IWaterResourceUpdateHandler _waterResourceUpdateHandler;
        private readonly IWaterResourceDeleteHandler _waterResourceDeleteHandler;
        private readonly IWaterResourceGetSingleHandler _waterResourceGetSingleHandler;
        private readonly IWaterResourceGetAllHandler _waterResourceGetAllHandler;

        public WaterResourceController(
            IUnitOfWork uow,
            IWaterResourceCreateHandler waterResourceCreateHandler,
            IWaterResourceUpdateHandler waterResourceUpdateHandler,
            IWaterResourceDeleteHandler waterResourceDeleteHandler,
            IWaterResourceGetSingleHandler waterResourceGetSingleHandler,
            IWaterResourceGetAllHandler waterResourceGetAllHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterResourceCreateHandler = waterResourceCreateHandler;
            _waterResourceCreateHandler.NotNull(nameof(waterResourceCreateHandler));

            _waterResourceUpdateHandler = waterResourceUpdateHandler;
            _waterResourceUpdateHandler.NotNull(nameof(waterResourceUpdateHandler));

            _waterResourceDeleteHandler = waterResourceDeleteHandler;
            _waterResourceDeleteHandler.NotNull(nameof(waterResourceDeleteHandler));

            _waterResourceGetSingleHandler = waterResourceGetSingleHandler;
            _waterResourceGetSingleHandler.NotNull(nameof(waterResourceGetSingleHandler));

            _waterResourceGetAllHandler = waterResourceGetAllHandler;
            _waterResourceGetAllHandler.NotNull(nameof(waterResourceGetAllHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterResourceCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] WaterResourceCreateDto createDto, CancellationToken cancellationToken)
        {
            await _waterResourceCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterResourceUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] WaterResourceUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _waterResourceUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterResourceDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] WaterResourceDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _waterResourceDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterResourceGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            WaterResourceGetDto waterResources = await _waterResourceGetSingleHandler.Handle(id, cancellationToken);
            return Ok(waterResources);
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WaterResourceGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            ICollection<WaterResourceGetDto> waterResources = await _waterResourceGetAllHandler.Handle(cancellationToken);
            return Ok(waterResources);
        }
    }
}
