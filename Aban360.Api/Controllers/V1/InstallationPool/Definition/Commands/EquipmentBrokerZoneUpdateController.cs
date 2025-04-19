using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Update.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition.Commands
{
    [Route("v1/equipment-broker-zone")]
    public class EquipmentBrokerZoneUpdateController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEquipmentBrokerZoneUpdateHandler _equipmentBrokerZoneUpdateHandler;
        public EquipmentBrokerZoneUpdateController(
            IUnitOfWork uow,
            IEquipmentBrokerZoneUpdateHandler equipmentBrokerZoneUpdateHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _equipmentBrokerZoneUpdateHandler = equipmentBrokerZoneUpdateHandler;
            _equipmentBrokerZoneUpdateHandler.NotNull(nameof(equipmentBrokerZoneUpdateHandler));
        }

        [HttpPost, HttpPatch]
        [Route("update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EquipmentBrokerZoneUpdateDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] EquipmentBrokerZoneUpdateDto updateDto, CancellationToken cancellationToken)
        {
            await _equipmentBrokerZoneUpdateHandler.Handle(updateDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(updateDto);
        }
    }
}
