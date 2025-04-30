using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("v1/watermeter-installation-method")]
    public class WaterMeterInstallationMethodGetSingleController : BaseController
    {
        private readonly IWaterMeterInstallationMethodGetSingleHandler _waterMeterInstallationMethodGetSingleHandler;
        public WaterMeterInstallationMethodGetSingleController(IWaterMeterInstallationMethodGetSingleHandler waterMeterInstallationMethodGetSingleHandler)
        {
            _waterMeterInstallationMethodGetSingleHandler = waterMeterInstallationMethodGetSingleHandler;
            _waterMeterInstallationMethodGetSingleHandler.NotNull(nameof(waterMeterInstallationMethodGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterInstallationMethodGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var waterMeterInstallationMethods = await _waterMeterInstallationMethodGetSingleHandler.Handle(id, cancellationToken);
            return Ok(waterMeterInstallationMethods);
        }
    }
}
