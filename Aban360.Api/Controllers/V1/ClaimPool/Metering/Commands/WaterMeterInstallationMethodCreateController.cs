using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/watermeter-installation-method")]
    public class WaterMeterInstallationMethodCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterInstallationMethodCreateHandler _waterMeterInstallationMethodCreateHandler;
        public WaterMeterInstallationMethodCreateController(
            IUnitOfWork uow,
            IWaterMeterInstallationMethodCreateHandler waterMeterInstallationMethodCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterMeterInstallationMethodCreateHandler = waterMeterInstallationMethodCreateHandler;
            _waterMeterInstallationMethodCreateHandler.NotNull(nameof(waterMeterInstallationMethodCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterInstallationMethodCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] WaterMeterInstallationMethodCreateDto createDto, CancellationToken cancellationToken)
        {
            await _waterMeterInstallationMethodCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
