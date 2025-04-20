using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Create.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition.Commands
{
    [Route("v1/equipment-broker-zone")]
    public class EquipmentBrokerZoneCreateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEquipmentBrokerZoneCreateHandler _equipmentBrokerZoneCreateHandler;
        public EquipmentBrokerZoneCreateController(
            IUnitOfWork uow,
            IEquipmentBrokerZoneCreateHandler equipmentBrokerZoneCreateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _equipmentBrokerZoneCreateHandler = equipmentBrokerZoneCreateHandler;
            _equipmentBrokerZoneCreateHandler.NotNull(nameof(equipmentBrokerZoneCreateHandler));
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EquipmentBrokerZoneCreateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] EquipmentBrokerZoneCreateDto createDto, CancellationToken cancellationToken)
        {
            await _equipmentBrokerZoneCreateHandler.Handle(createDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(createDto);
        }
    }
}
