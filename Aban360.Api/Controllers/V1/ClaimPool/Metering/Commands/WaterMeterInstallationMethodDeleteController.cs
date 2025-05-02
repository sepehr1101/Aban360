using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/watermeter-installation-method")]
    public class WaterMeterInstallationMethodDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterInstallationMethodDeleteHandler _waterMeterInstallationMethodDeleteHandler;
        public WaterMeterInstallationMethodDeleteController(
            IUnitOfWork uow,
            IWaterMeterInstallationMethodDeleteHandler waterMeterInstallationMethodDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterMeterInstallationMethodDeleteHandler = waterMeterInstallationMethodDeleteHandler;
            _waterMeterInstallationMethodDeleteHandler.NotNull(nameof(waterMeterInstallationMethodDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterInstallationMethodDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] WaterMeterInstallationMethodDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _waterMeterInstallationMethodDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
