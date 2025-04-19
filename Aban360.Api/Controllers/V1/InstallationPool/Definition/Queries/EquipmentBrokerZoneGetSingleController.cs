using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition.Queries
{
    [Route("v1/equipment_broker_zone")]
    public class EquipmentBrokerZoneGetSingleController : BaseController
    {
        private readonly IEquipmentBrokerZoneGetSingleHandler _equipmentBrokerZoneGetSingleHandler;
        public EquipmentBrokerZoneGetSingleController(IEquipmentBrokerZoneGetSingleHandler equipmentBrokerZoneGetSingleHandler)
        {
            _equipmentBrokerZoneGetSingleHandler = equipmentBrokerZoneGetSingleHandler;
            _equipmentBrokerZoneGetSingleHandler.NotNull(nameof(equipmentBrokerZoneGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<EquipmentBrokerZoneGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var equipmentBrokerZones = await _equipmentBrokerZoneGetSingleHandler.Handle(id, cancellationToken);
            return Ok(equipmentBrokerZones);
        }
    }
}
