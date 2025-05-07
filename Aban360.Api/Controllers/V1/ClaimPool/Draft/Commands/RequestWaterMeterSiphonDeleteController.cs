using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-water-meter-siphon")]
    public class RequestWaterMeterSiphonDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestWaterMeterSiphonDeleteHandler _requestWaterMeterSiphonDeleteHandler;
        public RequestWaterMeterSiphonDeleteController(
            IUnitOfWork uow,
            IRequestWaterMeterSiphonDeleteHandler requestWaterMeterSiphonDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestWaterMeterSiphonDeleteHandler = requestWaterMeterSiphonDeleteHandler;
            _requestWaterMeterSiphonDeleteHandler.NotNull(nameof(requestWaterMeterSiphonDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterSiphonRequestDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] WaterMeterSiphonRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _requestWaterMeterSiphonDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
