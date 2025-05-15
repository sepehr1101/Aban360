using Aban360.Common.Categories.ApiResponse;
using Microsoft.AspNetCore.Mvc;
using Aban360.Common.Extensions;
using Aban360.InstallationPool.Application.Features.Definition.Handlers.Queries.Contracts;
using Aban360.InstallationPool.Domain.Features.Definition.Dto.Queries;

namespace Aban360.Api.Controllers.V1.InstallationPool.Definition.Queries
{
    [Route("v1/sewage-equipment-broker")]
    public class SewageEquipmentBrokerGetSingleController : BaseController
    {
        private readonly ISewageEquipmentBrokerGetSingleHandler _sewageEquipmentBrokerGetSingleHandler;
        public SewageEquipmentBrokerGetSingleController(ISewageEquipmentBrokerGetSingleHandler sewageEquipmentBrokerGetSingleHandler)
        {
            _sewageEquipmentBrokerGetSingleHandler = sewageEquipmentBrokerGetSingleHandler;
            _sewageEquipmentBrokerGetSingleHandler.NotNull(nameof(sewageEquipmentBrokerGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<SewageEquipmentBrokerGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(short id, CancellationToken cancellationToken)
        {
            var sewageEquipmentBrokers = await _sewageEquipmentBrokerGetSingleHandler.Handle(id, cancellationToken);
            return Ok(sewageEquipmentBrokers);
        }
    }
}
