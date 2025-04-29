using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Update.Contracts;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/watermeter-installation-structure")]
    public class WaterMeterInstallationStructureUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterInstallationStructureUpdateHandler _waterMeterInstallationStructureUpdateHandler;
        public WaterMeterInstallationStructureUpdateController(
            IUnitOfWork uow,
            IWaterMeterInstallationStructureUpdateHandler waterMeterInstallationStructureUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterMeterInstallationStructureUpdateHandler = waterMeterInstallationStructureUpdateHandler;
            _waterMeterInstallationStructureUpdateHandler.NotNull(nameof(waterMeterInstallationStructureUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterInstallationStructureUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] WaterMeterInstallationStructureUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _waterMeterInstallationStructureUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
