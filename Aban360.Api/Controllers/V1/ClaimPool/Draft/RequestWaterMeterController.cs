using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Create.Contracts;
using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft
{
    [Route("v1/request-water-meter")]
    public class RequestWaterMeterController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestWaterMeterCreateHandler _requestWaterMeterCreateHandler;
        private readonly IRequestWaterMeterDeleteHandler _requestWaterMeterDeleteHandler;
        private readonly IRequestWaterMeterUpdateHandler _requestWaterMeterUpdateHandler;

        public RequestWaterMeterController(
            IUnitOfWork unitOfWork,
            IRequestWaterMeterCreateHandler requestWaterMeterCreateHandler,
            IRequestWaterMeterDeleteHandler requestWaterMeterDeleteHandler,
            IRequestWaterMeterUpdateHandler requestWaterMeterUpdateHandler)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.NotNull(nameof(unitOfWork));
            _requestWaterMeterCreateHandler = requestWaterMeterCreateHandler;
            _requestWaterMeterCreateHandler.NotNull(nameof(requestWaterMeterCreateHandler));
            _requestWaterMeterDeleteHandler = requestWaterMeterDeleteHandler;
            _requestWaterMeterDeleteHandler.NotNull(nameof(requestWaterMeterDeleteHandler));
            _requestWaterMeterUpdateHandler = requestWaterMeterUpdateHandler;
            _requestWaterMeterUpdateHandler.NotNull(nameof(requestWaterMeterUpdateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterRequestCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] WaterMeterRequestCreateDto createDto, CancellationToken cancellationToken)
        {
            await _requestWaterMeterCreateHandler.Handle(CurrentUser,createDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterRequestDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] WaterMeterRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _requestWaterMeterDeleteHandler.Handle(deleteDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterRequestUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] WaterMeterRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _requestWaterMeterUpdateHandler.Handle(CurrentUser,updateDto, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
