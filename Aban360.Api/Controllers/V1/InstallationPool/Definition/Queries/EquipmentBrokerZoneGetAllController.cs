using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition.Queries
{
    [Route("v1/equipment-broker-zone")]
    public class EquipmentBrokerZoneGetAllController : BaseController
    {
        private readonly IEquipmentBrokerZoneGetAllHandler _equipmentBrokerZoneGetAllHandler;
        public EquipmentBrokerZoneGetAllController(IEquipmentBrokerZoneGetAllHandler equipmentBrokerZoneGetAllHandler)
        {
            _equipmentBrokerZoneGetAllHandler = equipmentBrokerZoneGetAllHandler;
            _equipmentBrokerZoneGetAllHandler.NotNull(nameof(equipmentBrokerZoneGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<EquipmentBrokerZoneGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var equipmentBrokerZones = await _equipmentBrokerZoneGetAllHandler.Handle(cancellationToken);
            return Ok(equipmentBrokerZones);
        }
    }
}
