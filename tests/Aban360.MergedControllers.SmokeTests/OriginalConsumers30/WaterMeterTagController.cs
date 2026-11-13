// Frozen consumer before application service consolidation; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.ClaimPool.Metering;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Application.Features.WasteWater.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers30.Aban360.Api.Controllers.V1.ClaimPool.Metering
{
    public class WaterMeterTagController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWaterMeterTagCreateHandler _waterMeterTagCreateHandler;
        private readonly IWaterMeterTagDeleteHandler _waterMeterTagDeleteHandler;
        private readonly IWaterMeterTagUpdateHandler _waterMeterTagUpdateHandler;
        private readonly IWaterMeterTagGetSinglBySearchInputeHandler _waterMeterTagGetSinglBySearchInputeHandler;
        private readonly IWaterMeterTagGetSingleHandler _waterMeterTagGetSingleHandler;

        public WaterMeterTagController(
            IUnitOfWork unitOfWork,
            IWaterMeterTagCreateHandler waterMeterTagCreateHandler,
            IWaterMeterTagDeleteHandler waterMeterTagDeleteHandler,
            IWaterMeterTagUpdateHandler waterMeterTagUpdateHandler,
            IWaterMeterTagGetSinglBySearchInputeHandler waterMeterTagGetSinglBySearchInputeHandler,
            IWaterMeterTagGetSingleHandler waterMeterTagGetSingleHandler)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.NotNull(nameof(unitOfWork));
            _waterMeterTagCreateHandler = waterMeterTagCreateHandler;
            _waterMeterTagCreateHandler.NotNull(nameof(waterMeterTagCreateHandler));
            _waterMeterTagDeleteHandler = waterMeterTagDeleteHandler;
            _waterMeterTagDeleteHandler.NotNull(nameof(waterMeterTagDeleteHandler));
            _waterMeterTagUpdateHandler = waterMeterTagUpdateHandler;
            _waterMeterTagUpdateHandler.NotNull(nameof(waterMeterTagUpdateHandler));
            _waterMeterTagGetSinglBySearchInputeHandler = waterMeterTagGetSinglBySearchInputeHandler;
            _waterMeterTagGetSinglBySearchInputeHandler.NotNull(nameof(waterMeterTagGetSinglBySearchInputeHandler));
            _waterMeterTagGetSingleHandler = waterMeterTagGetSingleHandler;
            _waterMeterTagGetSingleHandler.NotNull(nameof(waterMeterTagGetSingleHandler));
        }

        [HttpPost]
        [Route("~/v1/water-meter-tag/create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterTagCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] WaterMeterTagCreateDto createDto, CancellationToken cancellationToken)
        {
            await _waterMeterTagCreateHandler.Handle(createDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpDelete]
        [Route("~/water-meter-tag/delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterTagDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] WaterMeterTagDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _waterMeterTagDeleteHandler.Handle(deleteDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpPatch]
        [Route("~/v1/water-meter-tag/update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterTagUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] WaterMeterTagUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _waterMeterTagUpdateHandler.Handle(updateDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }

        [HttpPost, HttpGet]
        [Route("~/water-meter-tag/search/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterTagGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search(string input, CancellationToken cancellationToken)
        {
            WaterMeterTagGetDto WaterMeterTag = await _waterMeterTagGetSinglBySearchInputeHandler.Handle(input, cancellationToken);
            return Ok(WaterMeterTag);
        }

        [HttpPost, HttpGet]
        [Route("~/water-meter-tag/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WaterMeterTagGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(int id, CancellationToken cancellationToken)
        {
            ICollection<WaterMeterTagGetDto> WaterMeterTag = await _waterMeterTagGetSingleHandler.Handle(id, cancellationToken);
            return Ok(WaterMeterTag);
        }
    }
}
