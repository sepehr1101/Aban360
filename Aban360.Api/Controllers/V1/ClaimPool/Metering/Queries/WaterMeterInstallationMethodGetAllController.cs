using Aban360.ClaimPool.Application.Features.Metering.Handlers.Queries.Contracts;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Queries
{
    [Route("v1/watermeter-installation-method")]
    public class WaterMeterInstallationMethodGetAllController : BaseController
    {
        private readonly IWaterMeterInstallationMethodGetAllHandler _waterMeterInstallationMethodGetAllHandler;
        public WaterMeterInstallationMethodGetAllController(IWaterMeterInstallationMethodGetAllHandler waterMeterInstallationMethodGetAllHandler)
        {
            _waterMeterInstallationMethodGetAllHandler = waterMeterInstallationMethodGetAllHandler;
            _waterMeterInstallationMethodGetAllHandler.NotNull(nameof(waterMeterInstallationMethodGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<WaterMeterInstallationMethodGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var waterMeterInstallationMethods = await _waterMeterInstallationMethodGetAllHandler.Handle(cancellationToken);
            return Ok(waterMeterInstallationMethods);
        }
    }
}
