using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.ClaimPool.Domain.Constants;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("v1/watermeter-installation-structure")]
    public class WaterMeterInstallationStructureGetSingleController : BaseController
    {
        private readonly IWaterMeterInstallationStructureGetSingleHandler _waterMeterInstallationStructureGetSingleHandler;
        public WaterMeterInstallationStructureGetSingleController(IWaterMeterInstallationStructureGetSingleHandler waterMeterInstallationStructureGetSingleHandler)
        {
            _waterMeterInstallationStructureGetSingleHandler = waterMeterInstallationStructureGetSingleHandler;
            _waterMeterInstallationStructureGetSingleHandler.NotNull(nameof(waterMeterInstallationStructureGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterInstallationStructureGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(WaterMeterInstallationStructureEnum id, CancellationToken cancellationToken)
        {
            var waterMeterInstallationStructures = await _waterMeterInstallationStructureGetSingleHandler.Handle(id, cancellationToken);
            return Ok(waterMeterInstallationStructures);
        }
    }
}
