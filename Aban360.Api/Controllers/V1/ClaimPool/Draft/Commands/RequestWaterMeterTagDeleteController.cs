using Aban360.ClaimPool.Application.Features.Draft.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Draft.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Draft.Commands
{
    [Route("v1/request-water-meter-tag")]
    public class RequestWaterMeterTagDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IRequestWaterMeterTagDeleteHandler _requestWaterMeterTagDeleteHandler;
        public RequestWaterMeterTagDeleteController(
            IUnitOfWork uow,
            IRequestWaterMeterTagDeleteHandler requestWaterMeterTagDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _requestWaterMeterTagDeleteHandler = requestWaterMeterTagDeleteHandler;
            _requestWaterMeterTagDeleteHandler.NotNull(nameof(requestWaterMeterTagDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterTagRequestDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] WaterMeterTagRequestDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _requestWaterMeterTagDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
