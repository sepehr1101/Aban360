using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition.Queries
{
    [Route("v1/sewage-equipment-broker-zone")]
    public class SewageEquipmentBrokerZoneGetSingleController : BaseController
    {
        private readonly ISewageEquipmentBrokerZoneGetSingleHandler _sewageEquipmentBrokerZoneGetSingleHandler;
        public SewageEquipmentBrokerZoneGetSingleController(ISewageEquipmentBrokerZoneGetSingleHandler sewageEquipmentBrokerZoneGetSingleHandler)
        {
            _sewageEquipmentBrokerZoneGetSingleHandler = sewageEquipmentBrokerZoneGetSingleHandler;
            _sewageEquipmentBrokerZoneGetSingleHandler.NotNull(nameof(sewageEquipmentBrokerZoneGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SewageEquipmentBrokerZoneGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var sewageEquipmentBrokerZones = await _sewageEquipmentBrokerZoneGetSingleHandler.Handle(id, cancellationToken);
            return Ok(sewageEquipmentBrokerZones);
        }
    }
}
