using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("v1/watermeter-installation-structure")]
    public class WaterMeterInstallationStructureGetAllController : BaseController
    {
        private readonly IWaterMeterInstallationStructureGetAllHandler _waterMeterInstallationStructureGetAllHandler;
        public WaterMeterInstallationStructureGetAllController(IWaterMeterInstallationStructureGetAllHandler waterMeterInstallationStructureGetAllHandler)
        {
            _waterMeterInstallationStructureGetAllHandler = waterMeterInstallationStructureGetAllHandler;
            _waterMeterInstallationStructureGetAllHandler.NotNull(nameof(waterMeterInstallationStructureGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WaterMeterInstallationStructureGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var waterMeterInstallationStructures = await _waterMeterInstallationStructureGetAllHandler.Handle(cancellationToken);
            return Ok(waterMeterInstallationStructures);
        }
    }
}
