using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Create.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition.Commands
{
    [Route("v1/sewage-equipment-broker-zone")]
    public class SewageEquipmentBrokerZoneCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ISewageEquipmentBrokerZoneCreateHandler _sewageEquipmentBrokerZoneCreateHandler;
        public SewageEquipmentBrokerZoneCreateController(
            IUnitOfWork uow,
            ISewageEquipmentBrokerZoneCreateHandler sewageEquipmentBrokerZoneCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _sewageEquipmentBrokerZoneCreateHandler = sewageEquipmentBrokerZoneCreateHandler;
            _sewageEquipmentBrokerZoneCreateHandler.NotNull(nameof(sewageEquipmentBrokerZoneCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SewageEquipmentBrokerZoneCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] SewageEquipmentBrokerZoneCreateDto createDto, CancellationToken cancellationToken)
        {
            await _sewageEquipmentBrokerZoneCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
