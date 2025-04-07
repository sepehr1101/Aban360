using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request_water_meter")]
    public class RequestWaterMeterUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestWaterMeterUpdateHandler _requestWaterMeterUpdateHandler;
        public RequestWaterMeterUpdateController(
            IUnitOfWork uow,
            IRequestWaterMeterUpdateHandler requestWaterMeterUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestWaterMeterUpdateHandler = requestWaterMeterUpdateHandler;
            _requestWaterMeterUpdateHandler.NotNull(nameof(requestWaterMeterUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterRequestUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] WaterMeterRequestUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _requestWaterMeterUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
