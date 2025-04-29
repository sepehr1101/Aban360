using Aban360.ClaimPool.Persistence.Contexts.Contracts;
using Aban360.ClaimPool.Application.Features.Metering.Handlers.Commands.Create.Contracts;
using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.ClaimPool.Domain.Features.Metering.Dto.Commands;

namespace Aban360.Api.Controllers.V1.ClaimPool.Metering.Commands
{
    [Route("v1/watermeter-installation-structure")]
    public class WaterMeterInstallationStructureCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWaterMeterInstallationStructureCreateHandler _waterMeterInstallationStructureCreateHandler;
        public WaterMeterInstallationStructureCreateController(
            IUnitOfWork uow,
            IWaterMeterInstallationStructureCreateHandler waterMeterInstallationStructureCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _waterMeterInstallationStructureCreateHandler = waterMeterInstallationStructureCreateHandler;
            _waterMeterInstallationStructureCreateHandler.NotNull(nameof(waterMeterInstallationStructureCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<WaterMeterInstallationStructureCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] WaterMeterInstallationStructureCreateDto createDto, CancellationToken cancellationToken)
        {
            await _waterMeterInstallationStructureCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
