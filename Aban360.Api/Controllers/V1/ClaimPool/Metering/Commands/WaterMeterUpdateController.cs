using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/water-meter")]
    public class WaterMeterUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterUpdateHandler _waterMeterHandler;
        public WaterMeterUpdateController(
            IUnitOfWork uow,
            IWaterMeterUpdateHandler waterMeterHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterMeterHandler = waterMeterHandler;
            _waterMeterHandler.NotNull(nameof(waterMeterHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] WaterMeterUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _waterMeterHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
