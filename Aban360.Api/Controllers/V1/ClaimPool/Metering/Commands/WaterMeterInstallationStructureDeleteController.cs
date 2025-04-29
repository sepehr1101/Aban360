using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Delete.Contracts;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/watermeter-installation-structure")]
    public class WaterMeterInstallationStructureDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterInstallationStructureDeleteHandler _waterMeterInstallationStructureDeleteHandler;
        public WaterMeterInstallationStructureDeleteController(
            IUnitOfWork uow,
            IWaterMeterInstallationStructureDeleteHandler waterMeterInstallationStructureDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterMeterInstallationStructureDeleteHandler = waterMeterInstallationStructureDeleteHandler;
            _waterMeterInstallationStructureDeleteHandler.NotNull(nameof(waterMeterInstallationStructureDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterInstallationStructureDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] WaterMeterInstallationStructureDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _waterMeterInstallationStructureDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
