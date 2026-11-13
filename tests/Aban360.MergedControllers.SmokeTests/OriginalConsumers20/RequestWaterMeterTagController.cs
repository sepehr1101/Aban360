// Frozen original consumer; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Api.Controllers.V1.ClaimPool.Draft;
using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace BeforeServiceConsumers20.Aban360.Api.Controllers.V1.ClaimPool.Draft
{
    [Route("v1/request-water-meter-tag")]
    public class RequestWaterMeterTagController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestWaterMeterTagCreateHandler _requestWaterMeterTagCreateHandler;
        private readonly IRequestWaterMeterTagDeleteHandler _requestWaterMeterTagDeleteHandler;
        private readonly IRequestWaterMeterTagUpdateHandler _requestWaterMeterTagUpdateHandler;

        public RequestWaterMeterTagController(
            IUnitOfWork unitOfWork,
            IRequestWaterMeterTagCreateHandler requestWaterMeterTagCreateHandler,
            IRequestWaterMeterTagDeleteHandler requestWaterMeterTagDeleteHandler,
            IRequestWaterMeterTagUpdateHandler requestWaterMeterTagUpdateHandler)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.NotNull(nameof(unitOfWork));
            _requestWaterMeterTagCreateHandler = requestWaterMeterTagCreateHandler;
            _requestWaterMeterTagCreateHandler.NotNull(nameof(requestWaterMeterTagCreateHandler));
            _requestWaterMeterTagDeleteHandler = requestWaterMeterTagDeleteHandler;
            _requestWaterMeterTagDeleteHandler.NotNull(nameof(requestWaterMeterTagDeleteHandler));
            _requestWaterMeterTagUpdateHandler = requestWaterMeterTagUpdateHandler;
            _requestWaterMeterTagUpdateHandler.NotNull(nameof(requestWaterMeterTagUpdateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterTagRequestCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] WaterMeterTagRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            await _requestWaterMeterTagCreateHandler.Handle(createDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterTagRequestDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] WaterMeterTagRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _requestWaterMeterTagDeleteHandler.Handle(deleteDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterTagRequestUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] WaterMeterTagRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _requestWaterMeterTagUpdateHandler.Handle(updateDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
