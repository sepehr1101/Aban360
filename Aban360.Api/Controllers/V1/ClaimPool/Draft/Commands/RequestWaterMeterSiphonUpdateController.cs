using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request_water_meter_siphon")]
    public class RequestWaterMeterSiphonUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestWaterMeterSiphonUpdateHandler _requestWaterMeterSiphonUpdateHandler;
        public RequestWaterMeterSiphonUpdateController(
            IUnitOfWork uow,
            IRequestWaterMeterSiphonUpdateHandler requestWaterMeterSiphonUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestWaterMeterSiphonUpdateHandler = requestWaterMeterSiphonUpdateHandler;
            _requestWaterMeterSiphonUpdateHandler.NotNull(nameof(requestWaterMeterSiphonUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterSiphonRequestUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] WaterMeterSiphonRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _requestWaterMeterSiphonUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
