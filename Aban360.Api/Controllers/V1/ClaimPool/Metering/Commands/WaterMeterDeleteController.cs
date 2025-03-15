using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/water-meter")]
    public class WaterMeterDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterDeleteHandler _waterMeterHandler;
        public WaterMeterDeleteController(
            IUnitOfWork uow,
            IWaterMeterDeleteHandler waterMeterHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterMeterHandler = waterMeterHandler;
            _waterMeterHandler.NotNull(nameof(waterMeterHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] WaterMeterDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _waterMeterHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
