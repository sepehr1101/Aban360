using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition.Queries
{
    [Route("v1/sewage-equipment-broker-zone")]
    public class SewageEquipmentBrokerZoneGetAllController : BaseController
    {
        private readonly ISewageEquipmentBrokerZoneGetAllHandler _sewageEquipmentBrokerZoneGetAllHandler;
        public SewageEquipmentBrokerZoneGetAllController(ISewageEquipmentBrokerZoneGetAllHandler sewageEquipmentBrokerZoneGetAllHandler)
        {
            _sewageEquipmentBrokerZoneGetAllHandler = sewageEquipmentBrokerZoneGetAllHandler;
            _sewageEquipmentBrokerZoneGetAllHandler.NotNull(nameof(sewageEquipmentBrokerZoneGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<SewageEquipmentBrokerZoneGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var sewageEquipmentBrokerZones = await _sewageEquipmentBrokerZoneGetAllHandler.Handle(cancellationToken);
            return Ok(sewageEquipmentBrokerZones);
        }
    }
}
