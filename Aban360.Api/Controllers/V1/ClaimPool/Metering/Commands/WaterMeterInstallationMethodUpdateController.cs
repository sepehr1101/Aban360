using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/watermeter-installation-method")]
    public class WaterMeterInstallationMethodUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterInstallationMethodUpdateHandler _waterMeterInstallationMethodUpdateHandler;
        public WaterMeterInstallationMethodUpdateController(
            IUnitOfWork uow,
            IWaterMeterInstallationMethodUpdateHandler waterMeterInstallationMethodUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterMeterInstallationMethodUpdateHandler = waterMeterInstallationMethodUpdateHandler;
            _waterMeterInstallationMethodUpdateHandler.NotNull(nameof(waterMeterInstallationMethodUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterInstallationMethodUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] WaterMeterInstallationMethodUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _waterMeterInstallationMethodUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
