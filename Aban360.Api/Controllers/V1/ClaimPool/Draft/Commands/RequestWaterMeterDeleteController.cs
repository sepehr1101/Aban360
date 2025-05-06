using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-water-meter")]
    public class RequestWaterMeterDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestWaterMeterDeleteHandler _requestWaterMeterDeleteHandler;
        public RequestWaterMeterDeleteController(
            IUnitOfWork uow,
            IRequestWaterMeterDeleteHandler requestWaterMeterDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestWaterMeterDeleteHandler = requestWaterMeterDeleteHandler;
            _requestWaterMeterDeleteHandler.NotNull(nameof(requestWaterMeterDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterRequestDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] WaterMeterRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _requestWaterMeterDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
