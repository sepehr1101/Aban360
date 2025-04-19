using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Commands.Delete.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Commands;
using Aban360.InstallationPool.Persistence.Contexts.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition.Commands
{
    [Route("v1/equipment_broker_zone")]
    public class EquipmentBrokerZoneDeleteController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEquipmentBrokerZoneDeleteHandler _equipmentBrokerZoneDeleteHandler;
        public EquipmentBrokerZoneDeleteController(
            IUnitOfWork uow,
            IEquipmentBrokerZoneDeleteHandler equipmentBrokerZoneDeleteHandler)
        {
            _uow = uow;
            _uow.NotNull(nameof(uow));

            _equipmentBrokerZoneDeleteHandler = equipmentBrokerZoneDeleteHandler;
            _equipmentBrokerZoneDeleteHandler.NotNull(nameof(equipmentBrokerZoneDeleteHandler));
        }

        [HttpPost, HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EquipmentBrokerZoneDeleteDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromBody] EquipmentBrokerZoneDeleteDto deleteDto, CancellationToken cancellationToken)
        {
            await _equipmentBrokerZoneDeleteHandler.Handle(deleteDto, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return Ok(deleteDto);
        }
    }
}
